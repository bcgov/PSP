#!/usr/bin/env bash
#
# common.sh
#
# Shared helpers for the PIMS CI/CD scripts. Source it, do not execute it:
#
#   SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
#   . "${SCRIPT_DIR}/common.sh"
#
# ---------------------------------------------------------------------------
# oc_retry
# ---------------------------------------------------------------------------
# Runs `oc` and retries on ANY non-zero exit, with exponential backoff.
#
# The Silver cluster's API endpoint is intermittently unreachable. Observed
# failures include:
#
#   The connection to the server api.silver.devops.gov.bc.ca:6443 was refused
#   dial tcp 142.34.194.119:6443: i/o timeout
#
# The trade-off, accepted deliberately: a genuinely failing call (bad
# permissions, a resource that does not exist) now takes the full backoff
# before reporting. With the defaults that is ~14s per call. Keep oc_retry off
# calls where failure is an expected, frequent outcome; see the note below.
#
# Retries are safe for the calls this wraps: `oc tag` is idempotent, and the
# rest are reads.
#
# stdout is passed through untouched so callers can capture a value with $( ).
# Only stderr is captured, which matters because oc prints an unrelated
# "Warning: Use tokens from the TokenRequest API ..." line on every call.
#
# Env vars:
#   OC_RETRY_ATTEMPTS  total attempts, including the first. Default 4.
#                      Set to 1 to disable retrying for a single call.
#   OC_RETRY_DELAY     seconds before the first retry. Doubles each time.
#                      Default 2, so waits are 2s, 4s, 8s (14s worst case).
#
# Usage:
#   oc_retry -n "$NS" tag "$SOURCE" "$TARGET"
#   VALUE="$(oc_retry -n "$NS" get istag "$TAG" -o jsonpath='{...}')"
#
# Do NOT wrap:
#   - Long-running streaming commands (`start-build --wait --follow`,
#     `rollout status`). Capturing their stderr hides progress, and retrying a
#     build is wrong.
#   - Calls where a non-zero exit is an EXPECTED outcome, not a failure. In
#     this repo that is `oc cancel-build` (exits non-zero when there is
#     nothing to cancel) and the lookup of a version tag that is supposed to
#     not exist yet. Wrapping those spends the full backoff on the common path.
#
# Do wrap existence checks where absence means the run cannot continue, such
# as the source tag of a promotion. Those must not suppress oc's stderr
# either: with 2>&1 discarding the error, an API blip is indistinguishable
# from a genuinely missing resource and gets reported as the wrong thing.
#

oc_retry() {
  local oc_args=("$@")
  local attempts="${OC_RETRY_ATTEMPTS:-4}"
  local delay="${OC_RETRY_DELAY:-2}"
  local attempt=1
  local err_file rc

  err_file="$(mktemp)"

  while :; do
    # Capture the exit status explicitly. Do NOT write this as
    #   if oc "${oc_args[@]}"; then ... fi
    #   rc=$?
    # because an `if` whose condition is false and which has no `else` exits 0,
    # so rc would always be 0 and this function would report success after
    # exhausting every attempt.
    rc=0
    oc "${oc_args[@]}" 2>"${err_file}" || rc=$?

    if [[ "${rc}" -eq 0 ]]; then
      # Pass warnings through so nothing is silently swallowed.
      [[ -s "${err_file}" ]] && cat "${err_file}" >&2
      rm -f "${err_file}"
      return 0
    fi

    if [[ "${attempt}" -ge "${attempts}" ]]; then
      [[ -s "${err_file}" ]] && cat "${err_file}" >&2
      rm -f "${err_file}"
      return "${rc}"
    fi

    echo "WARN: 'oc ${oc_args[*]}' failed (attempt ${attempt}/${attempts}), retrying in ${delay}s" >&2
    [[ -s "${err_file}" ]] && sed 's/^/      /' "${err_file}" >&2
    sleep "${delay}"
    delay=$((delay * 2))
    attempt=$((attempt + 1))
  done
}
