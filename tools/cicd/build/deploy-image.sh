#!/usr/bin/env bash
#
# deploy-image.sh
#
# Promotes images to an environment: moves the mutable environment tag, then
# restarts the matching Deployment and waits for the rollout. Replaces the
# oc tag / rollout restart / rollout status triplet repeated across the
# github actions.
#
# Usage:
#   deploy-image.sh --from <source-tag> --env <env> [options] <image>...
#
#   --from <tag>     Required. Source tag to promote from, e.g. latest-dev,
#                    dev, test, or an immutable version tag (6.5.0.2-<env>).
#   --env <env>      Required. Target environment: dev, test, uat or prod.
#                    Sets the destination tag and the Deployment namespace.
#   --no-suffix      Treat each argument as the exact Deployment name instead
#                    of appending -<env>.
#   --no-rollout     Tag only. Skip restart and status for every image in this
#                    invocation, for images with no Deployment.
#   --timeout <secs> Rollout status timeout per image. Default 600.
#   --dry-run        Show what would happen without changing anything. Still
#                    contacts the cluster to confirm the source tag and the
#                    target Deployment both exist.
#
# Examples:
#   # DEV deployment (after images have been built):
#   deploy-image.sh --from latest-dev --env dev pims-app pims-api pims-proxy pims-scheduler
#
#   # DEV -> TEST promotion:
#   deploy-image.sh --from dev --env test pims-app pims-api pims-proxy pims-scheduler
#
#   # UAT -> PROD promotion (via immutable version tag):
#   deploy-image.sh --from 6.5.0.2-uat --env prod pims-app pims-api pims-proxy pims-scheduler
#
#   # mayan-bcgov has no Deployment, so it is tag-only in every environment
#   deploy-image.sh --from test --env uat --no-rollout mayan-bcgov
#
# PIMS environments do not map one-to-one onto namespaces.
#
#   env    namespace      Deployment
#   ----   ------------   ---------------
#   dev    3cd915-dev     <image>-dev       e.g. pims-api-dev, pims-proxy-dev
#   test   3cd915-dev     <image>-test
#   uat    3cd915-test    <image>-uat
#   prod   3cd915-prod    <image>-prod
#
# Every image is processed even if one fails; the exit status is non-zero if
# any did, and the summary reports how many of the requested images succeeded.
#
# Env vars:
#   PROJ_TOOLS  namespace holding the ImageStreams. Default 3cd915-tools.
#
# Requires:
#   oc   already available via redhat-actions/oc-login
#
set -euo pipefail

# Shared helpers (oc_retry). Resolved relative to this script so it works
# regardless of the caller's working directory.
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# shellcheck source=common.sh
. "${SCRIPT_DIR}/common.sh"

PROJ_TOOLS="${PROJ_TOOLS:-3cd915-tools}"

SOURCE_TAG=""
ENV_NAME=""
TIMEOUT="600"
NO_SUFFIX=false
NO_ROLLOUT=false
DRY_RUN=false

usage() {
  cat >&2 <<'EOF'
Usage: deploy-image.sh --from <source-tag> --env <env> [options] <image>...

  --from <tag>     Required. Source tag to promote from.
  --env <env>      Required. dev, test, uat or prod.
  --no-suffix      Arguments are exact Deployment names, do not append -<env>.
  --no-rollout     Tag only, skip restart and status.
  --timeout <secs> Rollout status timeout per image. Default 600.
  --dry-run        Show what would happen without changing anything.

Example:
  # DEV -> TEST promotion:
  deploy-image.sh --from dev --env test pims-app pims-api pims-proxy pims-scheduler
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

if ! command -v oc >/dev/null 2>&1; then
  echo "ERROR: 'oc' is required but was not found on PATH." >&2
  exit 1
fi

while [[ "$#" -gt 0 ]]; do
  case "$1" in
  --from)
    require_value "--from" "$#"
    SOURCE_TAG="$2"
    shift 2
    ;;
  --from=*)
    SOURCE_TAG="${1#--from=}"
    shift
    ;;
  --env)
    require_value "--env" "$#"
    ENV_NAME="$2"
    shift 2
    ;;
  --env=*)
    ENV_NAME="${1#--env=}"
    shift
    ;;
  --timeout)
    require_value "--timeout" "$#"
    TIMEOUT="$2"
    shift 2
    ;;
  --timeout=*)
    TIMEOUT="${1#--timeout=}"
    shift
    ;;
  --no-suffix)
    NO_SUFFIX=true
    shift
    ;;
  --no-rollout)
    NO_ROLLOUT=true
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

[[ -n "${SOURCE_TAG}" ]] || {
  echo "ERROR: --from is required" >&2
  usage
}
[[ -n "${ENV_NAME}" ]] || {
  echo "ERROR: --env is required" >&2
  usage
}
[[ "$#" -ge 1 ]] || {
  echo "ERROR: at least one image name is required" >&2
  usage
}

if ! [[ "${TIMEOUT}" =~ ^[0-9]+$ ]]; then
  echo "ERROR: --timeout must be a whole number of seconds, got '${TIMEOUT}'" >&2
  exit 1
fi

case "${ENV_NAME}" in
dev) NAMESPACE="3cd915-dev" ;;
test) NAMESPACE="3cd915-dev" ;;
uat) NAMESPACE="3cd915-test" ;;
prod) NAMESPACE="3cd915-prod" ;;
*)
  echo "ERROR: --env must be dev, test, uat or prod, got '${ENV_NAME}'" >&2
  exit 1
  ;;
esac

IMAGES=("$@")
REQUESTED="${#IMAGES[@]}"
SUCCEEDED=0
FAILURES=0

echo "deploy-image.sh: promoting ${REQUESTED} image(s)"
echo "  source tag  : ${SOURCE_TAG}"
echo "  target tag  : ${ENV_NAME}"
echo "  imagestreams: ${PROJ_TOOLS}"
echo "  deployments : ${NAMESPACE}"
echo "  rollout     : $([[ "${NO_ROLLOUT}" = true ]] && echo "skipped (--no-rollout)" || echo "restart, timeout ${TIMEOUT}s")"
echo

for IMAGE in "${IMAGES[@]}"; do
  if [[ "${NO_SUFFIX}" = true ]]; then
    DEPLOYMENT="${IMAGE}"
  else
    DEPLOYMENT="${IMAGE}-${ENV_NAME}"
  fi

  SOURCE="${IMAGE}:${SOURCE_TAG}"
  TARGET="${IMAGE}:${ENV_NAME}"

  echo "--- ${IMAGE} ---"

  # Fail before tagging rather than creating a destination tag that points at
  # nothing and then restarting into a broken state.
  if ! oc_retry -n "${PROJ_TOOLS}" get istag "${SOURCE}" >/dev/null; then
    echo "ERROR: could not confirm source tag ${SOURCE} in ${PROJ_TOOLS}" >&2
    FAILURES=$((FAILURES + 1))
    continue
  fi

  # Checked before the dry-run branch so --dry-run is a real pre-flight: a
  # missing Deployment fails here rather than looking like a pass. In a real
  # run this also means the check happens BEFORE the tag is moved, so a bad
  # Deployment name no longer leaves the environment tag pointing at a new
  # image that nothing restarted onto.
  if [[ "${NO_ROLLOUT}" = false ]] && ! oc_retry -n "${NAMESPACE}" get "deployment/${DEPLOYMENT}" >/dev/null; then
    echo "ERROR: could not confirm deployment/${DEPLOYMENT} in ${NAMESPACE}." >&2
    echo "       Use --no-suffix if the argument is already the full Deployment" >&2
    echo "       name, or --no-rollout if this image has no Deployment." >&2
    FAILURES=$((FAILURES + 1))
    continue
  fi

  if [[ "${DRY_RUN}" = true ]]; then
    echo "DRY-RUN: oc -n ${PROJ_TOOLS} tag ${SOURCE} ${TARGET}"
    if [[ "${NO_ROLLOUT}" = false ]]; then
      echo "DRY-RUN: oc -n ${NAMESPACE} rollout restart deployment/${DEPLOYMENT}"
      echo "DRY-RUN: oc -n ${NAMESPACE} rollout status --timeout=${TIMEOUT}s deployment/${DEPLOYMENT}"
    fi
    SUCCEEDED=$((SUCCEEDED + 1))
    echo
    continue
  fi

  echo "TAG: ${SOURCE} -> ${TARGET}"
  if ! oc_retry -n "${PROJ_TOOLS}" tag "${SOURCE}" "${TARGET}"; then
    echo "ERROR: failed to tag ${SOURCE} -> ${TARGET}" >&2
    FAILURES=$((FAILURES + 1))
    continue
  fi

  if [[ "${NO_ROLLOUT}" = true ]]; then
    echo "SKIP: rollout not requested for ${IMAGE}"
    SUCCEEDED=$((SUCCEEDED + 1))
    echo
    continue
  fi

  echo "RESTART: deployment/${DEPLOYMENT} in ${NAMESPACE}"
  if ! oc_retry -n "${NAMESPACE}" rollout restart "deployment/${DEPLOYMENT}"; then
    echo "ERROR: rollout restart failed for deployment/${DEPLOYMENT}" >&2
    FAILURES=$((FAILURES + 1))
    continue
  fi

  echo "WAIT: deployment/${DEPLOYMENT} (timeout ${TIMEOUT}s)"
  if ! oc -n "${NAMESPACE}" rollout status --timeout="${TIMEOUT}s" "deployment/${DEPLOYMENT}"; then
    echo "ERROR: rollout did not complete for deployment/${DEPLOYMENT}" >&2
    FAILURES=$((FAILURES + 1))
    continue
  fi

  SUCCEEDED=$((SUCCEEDED + 1))
  echo "OK: ${IMAGE} -> ${TARGET}, deployment/${DEPLOYMENT} rolled out"
  echo
done

echo "deploy-image.sh: ${SUCCEEDED}/${REQUESTED} image(s) succeeded, ${FAILURES} failed."

if [[ "${FAILURES}" -gt 0 ]]; then
  exit 1
fi
