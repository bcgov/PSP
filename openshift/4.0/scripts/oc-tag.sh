#!/bin/bash
#
source "$(dirname ${0})/common.sh"

#%
#% OpenShift Tag Helper
#%
#%   This script tags the frontend and api images with the version set in git
#%
#%   Targets builds incl.: 'api', 'app-base' and 'app'
#%   OC Job Name: Job identifier (i.e. 'test' or 'uat') -- defaults to 'test'
#%
#% Usage:
#%
#%    ${THIS_FILE} [OC_JOB_NAME]
#%
#% Examples:
#%
#%   Provide an environment where images will be tagged 
#%    ${THIS_FILE} test
#%

# Receive parameters
#
OC_JOB_NAME=${1:-test}

# Variables
APP_NAME="pims";
IMG_BACKEND="${APP_NAME}-api";
IMG_FRONTEND="${APP_NAME}-app";
BUILD_CONFIG_IMG_TAG="latest-${OC_JOB_NAME}"; # image tag on build config is based on environment, e.g. latest-dev, latest-test, latest-uat

# Get version
REV_LIST=$(git rev-list --tags --max-count=1);
echo "${REV_LIST}";
VERSION=$(git describe --tags ${REV_LIST})
echo "${VERSION}";

# Exit if no version found
if [ -z "$VERSION" ]; 
  then 
    echo "*** ERROR, No version found ***";
    exit 1;
fi

# Set release version tag
RELEASE_VERSION_TAG="${VERSION}-${OC_JOB_NAME}"; #release version convention e.g Release-OC_JOB_NAME pims-api:v0.2.0.7.3-test or pims-api:v0.2.0.7.3-master
echo "${RELEASE_VERSION_TAG}";
TAG_BACKEND_COMMAND="oc tag ${IMG_BACKEND}:latest-${OC_JOB_NAME} ${IMG_BACKEND}:${RELEASE_VERSION_TAG}"
TAG_FRONTEND_COMMAND="oc tag ${IMG_FRONTEND}:latest-${OC_JOB_NAME} ${IMG_FRONTEND}:${RELEASE_VERSION_TAG}"
echo "${TAG_BACKEND_COMMAND}";
echo "${TAG_FRONTEND_COMMAND}";

# Execute tag commands
echo "Tagging images ${IMG_BACKEND}:latest-${OC_JOB_NAME} to ${IMG_BACKEND}:${RELEASE_VERSION_TAG}, ${IMG_FRONTEND}:latest-${OC_JOB_NAME} to ${IMG_FRONTEND}:${RELEASE_VERSION_TAG}"
eval "${TAG_BACKEND_COMMAND}"
eval "${TAG_FRONTEND_COMMAND}"

# Provide oc command instructions
display_helper $TAG_BACKEND_COMMAND $TAG_FRONTEND_COMMAND
