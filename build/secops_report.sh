#!/bin/bash
# secops_report.sh: a script that displays the results of code scanning tool trufflehog
# in a human-readable table

set -euo pipefail

usage() {
  cat <<EOF
Displays the results of code scanning tool trufflehog in a human-readable table.

Usage: $(basename $0) filename
EOF
  exit 0
}

# call usage() function if filename not supplied
if [ $# -eq 0 ]; then
  usage
else
  FILENAME="${1:-}"
fi

LINECOUNT=$(cat "$FILENAME" | jq -c '. | length')

if [ "$LINECOUNT" -eq 0 ]; then
  echo ":white_check_mark: No secrets were detected in the code."
else
  echo ":lock: The security scan detected ${LINECOUNT} potential secrets in the code."
  echo
  echo '```'
  cat "$FILENAME" | jq -c '[ .[] | { path, line:(.line)|tonumber, secret:(.secret)|(.[0:15]+"...") } ] | sort_by(.path, .line)' | jtbl
  echo '```'
fi
