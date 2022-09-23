#!/bin/bash
#
source "$(dirname ${0})/common.sh"

#%
#% OpenShift Deploy Helper
#%
#%   This command starts a new deployment for the provided target deployment
#%   Target deployments incl.: 'api', 'app-base' and 'app'
#%   Target environments incl.: 'dev', 'test', 'prod'
#%
#% Usage:
#%
#%   [RELEASE_TAG=<>] ${THIS_FILE} [DEPLOY_NAME] [ENV_NAME] [-apply]
#%
#% Examples:
#%
#%   Provide a target deployment. Defaults to a dry-run.
#%   ${THIS_FILE} api dev
#%
#%   Apply when satisfied.
#%   ${THIS_FILE} api dev -apply
#%
#%   Set variables to non-defaults at runtime.  E.g. to deploy from TEST to PROD:
#%   RELEASE_TAG=test ${THIS_FILE} api prod -apply

# Receive parameters (source and destination)
#
SHORTNAME=${1:-}
ENVIRONMENT_NAME="${2:-dev}"
# Image tag on build config is based on environment, e.g. latest-dev, latest-test, latest-uat
IMG_TAG="latest-${ENVIRONMENT_NAME}"
RELEASE_TAG=${RELEASE_TAG:-$IMG_TAG}

# These two parameters allow the deployment of multiple "instances" to a single namespace
# E.g. to deploy a "test" instance to the DEV namespace in OpenShift:
#      INSTANCE = "-test" (or "-01", etc)
#      NAMESPACE_OVERRIDE = "3cd915-dev"
INSTANCE=${INSTANCE:-}
NAMESPACE_OVERRIDE=${NAMESPACE_OVERRIDE:-}

# Target namespace for deployments. Can be overwritten with NAMESPACE_OVERRIDE
TARGET_NAMESPACE="${NAMESPACE_OVERRIDE:-${PROJ_PREFIX}-${ENVIRONMENT_NAME}}"

# E.g. pims-api (no instance) OR
#      pims-api-test (INSTANCE = "-test") to deploy a "test" instance onto the DEV namespace in OCP
#
DEPLOYMENT_NAME="${APP_NAME}-${SHORTNAME}${INSTANCE}"

IMG_STREAM="${APP_NAME}-${SHORTNAME}"

# Trigger the deployment manually when both tags reference the same image hash - retagging won't trigger a deployment
#
HASH_SOURCE="$(oc -n ${PROJ_TOOLS} get istag ${IMG_STREAM}:${RELEASE_TAG} -o jsonpath='{.image.dockerImageReference}')"
HASH_TARGET="$(oc -n ${PROJ_TOOLS} get istag ${IMG_STREAM}:${ENVIRONMENT_NAME} -o jsonpath='{.image.dockerImageReference}')"
MANUAL_DEPLOY=$([ "${HASH_SOURCE:-}" != "${HASH_TARGET:-}" ] || echo true)

# Cancel all previous deployments
#
OC_CANCEL_ALL_PREV_DEPLOY="oc -n ${TARGET_NAMESPACE} rollout cancel dc/${DEPLOYMENT_NAME} || true"

# Deploy and follow the progress
#
OC_IMG_RETAG="oc -n ${PROJ_TOOLS} tag ${IMG_STREAM}:${RELEASE_TAG} ${IMG_STREAM}:${ENVIRONMENT_NAME}"
OC_DEPLOY="oc -n ${TARGET_NAMESPACE} rollout latest dc/${DEPLOYMENT_NAME}"
[ "${MANUAL_DEPLOY}" ] || OC_DEPLOY=""
OC_STATUS="oc -n ${TARGET_NAMESPACE} rollout status dc/${DEPLOYMENT_NAME} --watch"

if [ "${APPLY}" ]; then
  echo "canceling previous deployments..."
  eval "${OC_CANCEL_ALL_PREV_DEPLOY}"
  count=1
  timeout=10
  # Check previous deployment statuses before moving onto new deploying
  while [ $count -le $timeout ]; do
    sleep 1
    PENDINGS="$(oc -n ${TARGET_NAMESPACE} rollout history dc/${DEPLOYMENT_NAME} | awk '{print $2}' | grep -c Pending || true)"
    RUNNINGS="$(oc -n ${TARGET_NAMESPACE} rollout history dc/${DEPLOYMENT_NAME} | awk '{print $2}' | grep -c Running || true)"
    if [ "${PENDINGS}" == 0 ] && [ "${RUNNINGS}" == 0 ]; then
      # No pending or running replica controllers so exit the while loop
      break 2
    fi
    count=$(( $count + 1 ))
  done
  if [ $count -gt $timeout ]; then
    echo "\n*** Reached the timeout for canceling previous deployments ***\n"
    exit 1
  fi

  # Execute commands
  #
  eval "${OC_IMG_RETAG}"
  eval "${OC_DEPLOY}"
  eval "${OC_STATUS}"
fi

# Provide oc command instructions
#
display_helper $OC_CANCEL_ALL_PREV_DEPLOY $OC_IMG_RETAG $OC_DEPLOY $OC_STATUS
