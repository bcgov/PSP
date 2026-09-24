#!/usr/bin/env bash
#
# build-image.sh
#
# Starts an OpenShift build for one or more BuildConfigs, waits for each to
# complete, and stamps PIMS application version into the resulting images.
#
# Usage:
#   build-image.sh --env <env> --version <version> [options] <buildconfig>...
#
#   --env <env>      Required. dev, test, uat or prod. Stamped as PIMS_BUILD_ENV.
#                    Warns if it disagrees with the BuildConfig name suffix.
#   --version <v>    Required. <major>.<minor>.<hotfix>.<build>, e.g. 6.5.0.2.
#   --no-cancel      Do not cancel in-flight builds first.
#   --dry-run        Show what would happen without starting any build. Still
#                    contacts the cluster to confirm each BuildConfig exists
#                    and to report the image tag it writes to.
#
# Examples:
#   # app-base must precede app: the app build consumes its output
#   build-image.sh --env dev --version 6.5.0.2 pims-app-base-dev pims-app-dev
#
#   build-image.sh --env dev --version 6.5.0.2 pims-api-dev pims-proxy-dev pims-scheduler-dev
#
#   build-image.sh --env uat --version 6.5.1.1 pims-api-uat pims-proxy-uat pims-scheduler-uat
#
# BuildConfigs are named for the environment they target (pims-api-dev, pims-api-uat), not the branch they build from.
# Builds run sequentially in the order given.
#
# Written into the image:
#   PIMS_VERSION=<version>     e.g. 6.5.0.2
#   PIMS_BUILD_ENV=<env>       e.g. dev, or uat for a UAT hotfix
#
# Requires:
#   oc   already available via redhat-actions/oc-login
#   jq   preinstalled on GitHub-hosted ubuntu runners
#
set -euo pipefail

# Shared helpers (oc_retry). Resolved relative to this script so it works
# regardless of the caller's working directory.
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# shellcheck source=common.sh
. "${SCRIPT_DIR}/common.sh"

PROJ_TOOLS="${PROJ_TOOLS:-3cd915-tools}"

ENV_NAME=""
VERSION=""
NO_CANCEL=false
DRY_RUN=false

usage() {
  cat >&2 <<'EOF'
Usage: build-image.sh --env <env> --version <version> [options] <buildconfig>...

  --env <env>      Required. dev, test, uat or prod. Stamped as PIMS_BUILD_ENV.
  --version <v>    Required. e.g. 6.5.0.2. Stamped as PIMS_VERSION.
  --no-cancel      Do not cancel in-flight builds first.
  --dry-run        Show what would happen without starting any build.

Example:
  build-image.sh --env dev --version 6.5.0.2 pims-app-base-dev pims-app-dev
EOF
  exit 2
}

require_value() {
  local flag="$1"
  local remaining_args="$2"

  [[ "${remaining_args}" -ge 2 ]] || {
    echo "ERROR: ${flag} requires a value" >&2
    usage
  }
}

while [[ "$#" -gt 0 ]]; do
  case "$1" in
  --env)
    require_value "--env" "$#"
    ENV_NAME="$2"
    shift 2
    ;;
  --env=*)
    ENV_NAME="${1#--env=}"
    shift
    ;;
  --version)
    require_value "--version" "$#"
    VERSION="$2"
    shift 2
    ;;
  --version=*)
    VERSION="${1#--version=}"
    shift
    ;;
  --no-cancel)
    NO_CANCEL=true
    shift
    ;;
  --dry-run)
    DRY_RUN=true
    shift
    ;;
  --)
    shift
    break
    ;;
  -*)
    echo "ERROR: unknown flag '$1'" >&2
    usage
    ;;
  *) break ;;
  esac
done

[[ -n "${ENV_NAME}" ]] || {
  echo "ERROR: --env is required" >&2
  usage
}
[[ -n "${VERSION}" ]] || {
  echo "ERROR: --version is required" >&2
  usage
}
[[ "$#" -ge 1 ]] || {
  echo "ERROR: at least one BuildConfig name is required" >&2
  usage
}

case "${ENV_NAME}" in
dev | test | uat | prod) ;;
*)
  echo "ERROR: --env must be dev, test, uat or prod, got '${ENV_NAME}'" >&2
  exit 1
  ;;
esac

if ! command -v oc >/dev/null 2>&1; then
  echo "ERROR: oc is required but was not found on PATH." >&2
  exit 1
fi

if ! [[ "${VERSION}" =~ ^[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
  echo "ERROR: version '${VERSION}' does not match <major>.<minor>.<hotfix>.<build>" >&2
  echo "       expected something like 6.5.0.2" >&2
  exit 1
fi

if [[ "${DRY_RUN}" = false ]] && ! command -v jq >/dev/null 2>&1; then
  echo "ERROR: jq is required but was not found on PATH." >&2
  echo "       GitHub-hosted ubuntu runners include it by default." >&2
  echo "       Self-hosted Linux runner: sudo apt-get install -y jq" >&2
  echo "       Git Bash on Windows does not ship jq: winget install jqlang.jq" >&2
  exit 1
fi

# Fetches a built image's env array once, as a JSON array.
# Fetched once per image and parsed repeatedly rather than re-fetched per var.
read_image_env_json() {
  local istag="$1"

  oc_retry -n "${PROJ_TOOLS}" get istag "${istag}" \
    -o jsonpath='{.image.dockerImageMetadata.Config.Env}' 2>/dev/null || true
}

# Extracts one variable's value from that array.
env_value() {
  local env_json="$1"
  local var_name="$2"

  printf '%s' "${env_json}" |
    jq -r --arg key "${var_name}" '.[] | select(startswith($key + "=")) | ltrimstr($key + "=")' \
      2>/dev/null | tail -1 || true
}

# Target environment implied by a BuildConfig name's trailing segment.
# Only the final segment is read, so pims-app-base-dev yields "dev".
# Prints nothing when the suffix is not a recognised environment.
derive_env() {
  local bc_name="$1"
  local suffix="${bc_name##*-}"

  case "${suffix}" in
  dev | test | uat | prod) printf '%s' "${suffix}" ;;
  *) printf '' ;;
  esac
}

BUILD_CONFIGS=("$@")
REQUESTED="${#BUILD_CONFIGS[@]}"
SUCCEEDED=0
FAILURES=0

echo "build-image.sh: building ${REQUESTED} image(s)"
echo "  namespace : ${PROJ_TOOLS}"
echo "  build-env : ${ENV_NAME}"
echo "  version   : ${VERSION}"
echo

for BC_NAME in "${BUILD_CONFIGS[@]}"; do
  echo "--- ${BC_NAME} ---"

  # Warn but proceed: passing --env uat alongside pims-api-dev is usually a
  # mistake, but a BuildConfig outside the naming convention is legitimate.
  DERIVED_ENV="$(derive_env "${BC_NAME}")"
  if [[ -n "${DERIVED_ENV}" ]] && [[ "${DERIVED_ENV}" != "${ENV_NAME}" ]]; then
    echo "WARN: ${BC_NAME} targets '${DERIVED_ENV}' by name, but --env says '${ENV_NAME}'." >&2
  fi
  BUILD_ENV="${ENV_NAME}"

  # Resolved before the dry-run branch so --dry-run is a real pre-flight check:
  # a missing BuildConfig or a BuildConfig with no output tag fails here rather
  # than looking like a pass. This means --dry-run requires a cluster login,
  # matching deploy-image.sh and tag-version.sh.
  # Retried, and oc's own stderr is not suppressed, so an API blip is not
  # misreported as a missing BuildConfig.
  if ! oc_retry -n "${PROJ_TOOLS}" get "bc/${BC_NAME}" >/dev/null; then
    echo "ERROR: could not confirm BuildConfig ${BC_NAME} in ${PROJ_TOOLS}" >&2
    FAILURES=$((FAILURES + 1))
    continue
  fi

  # Ask the BuildConfig where it writes, before starting the build.
  OUTPUT_TAG="$(oc_retry -n "${PROJ_TOOLS}" get "bc/${BC_NAME}" -o jsonpath='{.spec.output.to.name}' 2>/dev/null || true)"
  if [[ -z "${OUTPUT_TAG}" ]]; then
    echo "ERROR: could not determine output ImageStreamTag for ${BC_NAME}" >&2
    FAILURES=$((FAILURES + 1))
    continue
  fi
  echo "OUTPUT: ${OUTPUT_TAG}"

  if [[ "${DRY_RUN}" = true ]]; then
    [[ "${NO_CANCEL}" = false ]] && echo "DRY-RUN: oc -n ${PROJ_TOOLS} cancel-build bc/${BC_NAME}"
    echo "DRY-RUN: oc -n ${PROJ_TOOLS} start-build ${BC_NAME} --wait --follow \\"
    echo "DRY-RUN:     --env PIMS_VERSION=${VERSION} --env PIMS_BUILD_ENV=${BUILD_ENV}"
    echo "DRY-RUN: verify PIMS_VERSION=${VERSION} in ${OUTPUT_TAG} image config"
    SUCCEEDED=$((SUCCEEDED + 1))
    echo
    continue
  fi

  # Clear anything in flight so a Serial run policy does not queue this build behind a stale one.
  if [[ "${NO_CANCEL}" = false ]]; then
    # Not oc_retry: cancel-build exits non-zero when there is nothing to
    # cancel, which is the usual case, and retrying would add the full backoff
    # to every build.
    oc -n "${PROJ_TOOLS}" cancel-build "bc/${BC_NAME}" || true
  fi

  echo "BUILD: starting ${BC_NAME} (PIMS_VERSION=${VERSION}, PIMS_BUILD_ENV=${BUILD_ENV})"
  if ! oc -n "${PROJ_TOOLS}" start-build "${BC_NAME}" --wait --follow \
    --env "PIMS_VERSION=${VERSION}" \
    --env "PIMS_BUILD_ENV=${BUILD_ENV}"; then
    echo "ERROR: build failed for ${BC_NAME}" >&2
    FAILURES=$((FAILURES + 1))
    continue
  fi

  # --wait returns non-zero on failure, but confirm the phase explicitly so a
  # cancelled build is not mistaken for a pass.
  LAST_VERSION="$(oc_retry -n "${PROJ_TOOLS}" get "bc/${BC_NAME}" -o jsonpath='{.status.lastVersion}' 2>/dev/null || true)"
  if [[ -z "${LAST_VERSION}" ]]; then
    echo "ERROR: could not read lastVersion for ${BC_NAME}" >&2
    FAILURES=$((FAILURES + 1))
    continue
  fi

  PHASE="$(oc_retry -n "${PROJ_TOOLS}" get "build/${BC_NAME}-${LAST_VERSION}" -o jsonpath='{.status.phase}' 2>/dev/null || true)"
  if [[ "${PHASE}" != "Complete" ]]; then
    echo "ERROR: build ${BC_NAME}-${LAST_VERSION} phase is '${PHASE}', expected 'Complete'" >&2
    FAILURES=$((FAILURES + 1))
    continue
  fi
  echo "BUILD: ${BC_NAME}-${LAST_VERSION} complete"

  # Confirm the version reached the image. A BuildConfig that declares an
  # empty PIMS_VERSION placeholder will bake in an empty string if --env is
  # ever dropped, so verify rather than assume.
  IMAGE_ENV_JSON="$(read_image_env_json "${OUTPUT_TAG}")"
  READBACK="$(env_value "${IMAGE_ENV_JSON}" PIMS_VERSION)"
  if [[ "${READBACK}" != "${VERSION}" ]]; then
    echo "ERROR: PIMS_VERSION did not reach the image config for ${OUTPUT_TAG}." >&2
    echo "       expected '${VERSION}', read back '${READBACK}'." >&2
    FAILURES=$((FAILURES + 1))
    continue
  fi

  COMMIT="$(env_value "${IMAGE_ENV_JSON}" OPENSHIFT_BUILD_COMMIT)"
  SUCCEEDED=$((SUCCEEDED + 1))
  echo "OK: ${BC_NAME} -> ${OUTPUT_TAG}"
  echo "    PIMS_VERSION=${VERSION} PIMS_BUILD_ENV=${BUILD_ENV} commit=${COMMIT:0:8}"
  echo
done

echo "build-image.sh: ${SUCCEEDED}/${REQUESTED} build(s) succeeded, ${FAILURES} failed."

if [[ "${FAILURES}" -gt 0 ]]; then
  exit 1
fi
