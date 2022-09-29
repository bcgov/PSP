#!/bin/bash
#
source "$(dirname ${0})/common.sh"

#%
#% Sonar Scanner for Javascript/Typescript
#%
#%   This command runs a sonar scan for a typescript codebase
#%   Targets incl.: pr-###, dev and master.
#%
#% Usage:
#%
#%   ${THIS_FILE} [JOB_NAME] [-apply]
#%
#% Examples:
#%
#%   Provide a job name. Defaults to a dry-run.
#%   ${THIS_FILE} dev
#%
#%   Apply when satisfied.
#%   ${THIS_FILE} dev -apply
#%
#%   Set variables to non-defaults at runtime.
#%   SONAR_URL=http://localhost:9000 SONAR_PROJECT_KEY=my-project-key ${THIS_FILE} dev -apply

# Receive parameters (source and destination)
#
TARGET=${TARGET:-dev}
SONAR_PROJECT_KEY=${SONAR_PROJECT_KEY:-pims-frontend-${TARGET}}
SONAR_PROJECT_NAME=${SONAR_PROJECT_NAME:-PIMS Frontend [${TARGET}]}
SONAR_URL=${SONAR_URL:-https://sonarqube-3cd915-tools.apps.silver.devops.gov.bc.ca}
ZAP_REPORT=${ZAP_REPORT:-}
HTML_ZAP_REPORT=${HTML_ZAP_REPORT:-}
SONAR_TOKEN=${SONAR_TOKEN:-}

FRONTEND_DIR="${FRONTEND_DIR:-../../../source/frontend}"

# Check requirements
#
[ "${SONAR_TOKEN}" ] || {
  fatal_error "SONAR_TOKEN environment value is not set"
}
[ "${SONAR_URL}" ] || {
  fatal_error "SONAR_URL environment value is not set"
}
which sonar-scanner >/dev/null 2>&1 || {
  fatal_error "sonar-scanner not installed on the PATH"
}

# Clean up any previous test run
#
rm -rf ".sonarqube/"

# Unit tests & coverage
#
CMD_TEST="npm run coverage"

# Begin analysis
#
CMD_SONAR_SCAN="sonar-scanner \
  -D'sonar.projectName=pims-frontend[${TARGET}]' \
  -D'sonar.projectKey=pims-frontend-${TARGET}' \
  -D'sonar.sources=.' \
  -D'sonar.host.url=${SONAR_URL}' \
  ${SONAR_TOKEN:+ -D'sonar.login=${SONAR_TOKEN}'} \
  ${ZAP_REPORT:+ -D'sonar.zaproxy.reportPath=${ZAP_REPORT}'} \
  ${HTML_ZAP_REPORT:+ -D'sonar.zaproxy.htmlReportPath=${HTML_ZAP_REPORT}'}"

# Execute commands
#
if [ "${APPLY}" ]; then
  pushd ${FRONTEND_DIR}
  eval "${CMD_TEST}"
  eval "${CMD_SONAR_SCAN}"
  popd
fi

display_helper "${CMD_TEST}" "${CMD_SONAR_SCAN}"
