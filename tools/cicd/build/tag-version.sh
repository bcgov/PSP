#!/usr/bin/env bash
#
# tag-version.sh
#
# Creates an immutable, version-bearing tag from an environment's mutable tag,
# so a specific build can be identified and redeployed later. Replaces the
# repeated oc tag blocks in the deployment workflows.
#
# Usage:
#   tag-version.sh --env <env> --version <version> [options] <image>...
#
#   --env <env>      Required. dev, test, uat or prod. Selects the source tag
#                    (<image>:<env>) and the suffix on the target tag.
#   --version <v>    Required. <major>.<minor>.<hotfix>.<build>, e.g. 6.5.0.2.
#                    A leading "v" is stripped, so a raw git tag can be passed
#                    straight through.
#   --clean          Target tag omits the environment suffix. For the single
#                    PROD release tag. Source is still <image>:<env>.
#   --dry-run        Show what would be tagged without tagging.
#
# Examples:
#   tag-version.sh --env dev --version 6.5.0.2 pims-app pims-api pims-proxy pims-scheduler
#
#   tag-version.sh --env uat --version 6.5.1.1 pims-app pims-api pims-proxy pims-scheduler mayan-bcgov
#
#   # PROD gets 6.5.0.2 rather than 6.5.0.2-prod
#   tag-version.sh --env prod --version 6.5.0.2 --clean pims-app pims-api pims-proxy pims-scheduler mayan-bcgov
#
# Per image:
#   SOURCE = <image>:<env>
#   TARGET = <image>:<version>-<env>   or   <image>:<version> with --clean
#
#   - Existing target pointing at a different image  -> refuses, does not overwrite an immutable tag.
#   - Existing target pointing at the same image  -> skipped, so a re-run is a no-op.
#   - Otherwise tagged, then verified.
#
# Version cross-check:
#   Images built by build-image.sh carry PIMS_VERSION in their image config.
#   When the source image has it and it disagrees with --version, this warns
#   and proceeds. An image with no PIMS_VERSION is not warned about, since
#   mayan-bcgov and other third-party images are legitimately built elsewhere.
#   Requires jq; the check is skipped with a warning when jq is unavailable.
#
# Every image is processed even if one fails; the exit status is non-zero if
# any did, and the summary reports how many of the requested images succeeded.
#
# Env vars:
#   PROJ_TOOLS  namespace holding the ImageStreams. Default 3cd915-tools.
#
# Requires:
#   oc   already available via redhat-actions/oc-login
#   jq   optional, used only for the version cross-check.
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
CLEAN=false
DRY_RUN=false

usage() {
  cat >&2 <<'EOF'
Usage: tag-version.sh --env <env> --version <version> [options] <image>...

  --env <env>      Required. dev, test, uat or prod.
  --version <v>    Required. e.g. 6.5.0.2.
  --clean          Target tag omits the env suffix (PROD release tag).
  --dry-run        Show what would be tagged without tagging.

Example:
  tag-version.sh --env test --version 6.5.0.2 pims-app pims-api pims-proxy pims-scheduler
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

# jq drives the PIMS_VERSION cross-check only. That is advisory, so a missing
# jq downgrades the check rather than blocking the tagging itself.
JQ_AVAILABLE=true
if ! command -v jq >/dev/null 2>&1; then
  JQ_AVAILABLE=false
fi

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
  --clean)
    CLEAN=true
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
  echo "ERROR: at least one image name is required" >&2
  usage
}

case "${ENV_NAME}" in
dev | test | uat | prod) ;;
*)
  echo "ERROR: --env must be dev, test, uat or prod, got '${ENV_NAME}'" >&2
  exit 1
  ;;
esac

VERSION="${VERSION#v}"

# Catch a malformed version here rather than creating a garbage tag that the
# immutability guard would then protect indefinitely.
if ! [[ "${VERSION}" =~ ^[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
  echo "ERROR: version '${VERSION}' does not match <major>.<minor>.<hotfix>.<build>" >&2
  echo "       expected something like 6.5.0.2" >&2
  exit 1
fi

# Digest of a tag that is EXPECTED to be absent (the version tag being
# created). Deliberately not retried: absence is the normal outcome, and
# retrying would spend the full backoff on every image of every run.
get_digest() {
  local istag="$1"

  oc -n "${PROJ_TOOLS}" get istag "${istag}" \
    -o jsonpath='{.image.dockerImageReference}' 2>/dev/null || true
}

# Digest of a tag that MUST exist (the environment tag being promoted).
# Retried, and oc's stderr is not suppressed: an API blip here previously
# looked identical to "tag absent" and was reported as a missing source tag.
get_source_digest() {
  local istag="$1"

  oc_retry -n "${PROJ_TOOLS}" get istag "${istag}" \
    -o jsonpath='{.image.dockerImageReference}' || true
}

# PIMS_VERSION baked into the image by build-image.sh. Prints nothing when the
# image does not carry one, which is expected for third-party images.
read_image_version() {
  local istag="$1"

  oc_retry -n "${PROJ_TOOLS}" get istag "${istag}" \
    -o jsonpath='{.image.dockerImageMetadata.Config.Env}' 2>/dev/null |
    jq -r '.[] | select(startswith("PIMS_VERSION=")) | ltrimstr("PIMS_VERSION=")' \
      2>/dev/null | tail -1 || true
}

IMAGES=("$@")
REQUESTED="${#IMAGES[@]}"
SUCCEEDED=0
FAILURES=0

echo "tag-version.sh: tagging ${REQUESTED} image(s)"
echo "  source tag  : <image>:${ENV_NAME}"
echo "  target tag  : $([[ "${CLEAN}" = true ]] && echo "<image>:${VERSION}" || echo "<image>:${VERSION}-${ENV_NAME}")"
echo "  imagestreams: ${PROJ_TOOLS}"
if [[ "${JQ_AVAILABLE}" = false ]]; then
  echo "  note        : jq not found, PIMS_VERSION cross-check skipped"
fi
echo

for IMAGE in "${IMAGES[@]}"; do
  SOURCE="${IMAGE}:${ENV_NAME}"
  if [[ "${CLEAN}" = true ]]; then
    TARGET="${IMAGE}:${VERSION}"
  else
    TARGET="${IMAGE}:${VERSION}-${ENV_NAME}"
  fi

  echo "--- ${IMAGE} ---"

  SOURCE_DIGEST="$(get_source_digest "${SOURCE}")"
  if [[ -z "${SOURCE_DIGEST}" ]]; then
    echo "ERROR: could not confirm source tag ${SOURCE} in ${PROJ_TOOLS}" >&2
    FAILURES=$((FAILURES + 1))
    continue
  fi

  # Advisory: catches a promotion being stamped with a version that does not
  # match what was actually built. Silent when the image carries no
  # PIMS_VERSION, as third-party images are not built by build-image.sh.
  if [[ "${JQ_AVAILABLE}" = true ]]; then
    IMAGE_VERSION="$(read_image_version "${SOURCE}")"
    if [[ -n "${IMAGE_VERSION}" ]] && [[ "${IMAGE_VERSION}" != "${VERSION}" ]]; then
      echo "WARN: ${SOURCE} was built as PIMS_VERSION=${IMAGE_VERSION}," >&2
      echo "      but is being tagged as ${VERSION}." >&2
    fi
  fi

  TARGET_DIGEST="$(get_digest "${TARGET}")"

  if [[ -n "${TARGET_DIGEST}" ]] && [[ "${TARGET_DIGEST}" != "${SOURCE_DIGEST}" ]]; then
    echo "ERROR: ${TARGET} already exists and points at a different image." >&2
    echo "       ${TARGET} -> ${TARGET_DIGEST}" >&2
    echo "       ${SOURCE} -> ${SOURCE_DIGEST}" >&2
    echo "       Refusing to overwrite an existing immutable tag." >&2
    FAILURES=$((FAILURES + 1))
    continue
  fi

  if [[ "${TARGET_DIGEST}" = "${SOURCE_DIGEST}" ]]; then
    echo "SKIP: ${TARGET} already matches ${SOURCE}"
    SUCCEEDED=$((SUCCEEDED + 1))
    echo
    continue
  fi

  if [[ "${DRY_RUN}" = true ]]; then
    echo "DRY-RUN: oc -n ${PROJ_TOOLS} tag ${SOURCE} ${TARGET}"
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

  # Guard against registry lag or an unexpected retag resolving elsewhere.
  VERIFY_DIGEST="$(get_digest "${TARGET}")"
  if [[ "${VERIFY_DIGEST}" != "${SOURCE_DIGEST}" ]]; then
    echo "ERROR: post-tag verification failed for ${TARGET}" >&2
    echo "       expected ${SOURCE_DIGEST}, got ${VERIFY_DIGEST}" >&2
    FAILURES=$((FAILURES + 1))
    continue
  fi

  SUCCEEDED=$((SUCCEEDED + 1))
  echo "OK: ${TARGET}"
  echo
done

echo "tag-version.sh: ${SUCCEEDED}/${REQUESTED} image(s) succeeded, ${FAILURES} failed."

if [[ "${FAILURES}" -gt 0 ]]; then
  exit 1
fi
