#!/bin/bash

set -euo pipefail

usage() {
  echo "usage: $(basename $0) <filename>"
  exit 1
}

error() {
  echo "ERROR:" "$@"
  exit 1
}

# Set vars
if [ $# -eq 0 ]; then
  usage
else
  FILENAME="${1:-}"
fi

LINECOUNT=$(cat "$FILENAME" | jq -c '. | length')

if [ "$LINECOUNT" -eq 0 ]; then
  echo "No secrets were detected in the code."
else
  echo ":warning: The security scan detected ${LINECOUNT} potential secrets in the code."
  echo
  echo '```'
  cat "$FILENAME" | jq -c '[ .[] | { path, line:(.line)|tonumber, secret:(.secret)|(.[0:15]+"...") } ] | sort_by(.path, .line)' | jtbl
  echo '```'
fi
