#!/usr/bin/env bash

# debug on fail
set -euo pipefail
#source ./filebash
#set -x
readonly PROJECT_PATH="/opt/app" #from docker container directory, see dockerfile
#readonly PROJECT_PATH="$(pwd)"
readonly SCRIPT_NAME="$(basename $0)"

# sensible defaults if not set
: "${DEDUPE:=true}"
: "${TERM_EXIT:=false}"
: "${SLEEP_TIME:=60}"
: "${EXPORT_TIME:=120}"
: "${GRACEFUL_EXIT_TIME:=120}"
: "${HOSTNAME:=}"
: "${STARTUP_TIME:=0}"
AZ_BLOB_URL=${AZ_BLOB_URL:-""}
AZ_BLOB_CONTAINER=${AZ_BLOB_CONTAINER:-""}
AZ_SAS_TOKEN=${AZ_SAS_TOKEN:-""}
declare -i elapse=0

#echo $PROJECT_PATH

# Includes
source ${PROJECT_PATH}/lib/s3-common.sh

DATE_NOW=$(date +"%Y%m%d")
AZ_VERSION="2020-04-08"
AZ_BLOB_TARGET="${AZ_BLOB_URL}/${AZ_BLOB_CONTAINER}/"
##
# Print help and exit
# Arguments:
#   $1 int exit code
# Output:
#   string help
##
printUsageAndExitWith() {
  printf "Usage:\n"
  printf "  $SCRIPT_NAME [-k key] [-s file] [-r region] resource_path\n"
  printf "  $SCRIPT_NAME -h\n"
  printf "Example:\n"
  printf "  $SCRIPT_NAME -k key -s secret -r eu-central-1 /bucket/file.ext\n"
  printf "Options:\n"
  printf "  -h,--help\tPrint this help\n"
  printf "  -k,--key\tAWS Access Key ID. Default to environment variable AWS_ACCESS_KEY_ID\n"
  printf "  -r,--region\tAWS S3 Region. Default to environment variable AWS_DEFAULT_REGION\n"
  printf "  -s,--secret\tFile containing AWS Secret Access Key. If not set, secret will be environment variable AWS_SECRET_ACCESS_KEY\n"
  printf "     --version\tShow version\n"
  exit $1
}

##
# Parse command line and set global variables
# Arguments:
#   $@ command line
# Globals:
#   AWS_ACCESS_KEY_ID     string
#   AWS_SECRET_ACCESS_KEY string
#   AWS_REGION            string
#   RESOURCE_PATH         string
##

parseCommandLine() {
  # Init globals
  AWS_HOST=${AWS_HOST:-""}
  AWS_REGION=${AWS_REGION:-"us-east-1"}
  AWS_HOST=${AWS_HOST:-""}
  AWS_ACCESS_KEY_ID=${AWS_ACCESS_KEY_ID:-""}
  AWS_SECRET_ACCESS_KEY=${AWS_SECRET_ACCESS_KEY:-""}

  # Parse options
  local remaining=
  local secretKeyFile=
  while [[ $# > 0 ]]; do
    local key="$1"
    case $key in
    -h | --help) printUsageAndExitWith 0 ;;
    -r | --region)
      assertArgument $@
      AWS_REGION=$2
      shift
      ;;
    -k | --key)
      assertArgument $@
      AWS_ACCESS_KEY_ID=$2
      shift
      ;;
    -s | --secret)
      assertArgument $@
      secretKeyFile=$2
      shift
      ;;
    -*)
      err "Unknown option $1"
      printUsageAndExitWith $INVALID_USAGE_EXIT_CODE
      ;;
    *) remaining="$remaining \"$key\"" ;;
    esac
    shift
  done

  # Set the non-parameters back into the positional parameters ($1 $2 ..)
  eval set -- $remaining

  # Read secret file if set
  if ! [[ -z "$secretKeyFile" ]]; then
    AWS_SECRET_ACCESS_KEY=$(processAWSSecretFile "$secretKeyFile")
  fi

  RESOURCE_PATH="/${AWS_BUCKET_NAME}/pims-app_${DATE_NOW}.gz"
  # Parse arguments
  if [[ -z $RESOURCE_PATH ]]; then
    info "You need to specify the resource path to download e.g. /bucket/file.ext"
    printUsageAndExitWith $INVALID_USAGE_EXIT_CODE
  fi

  assertResourcePath $RESOURCE_PATH
  #RESOURCE_PATH="$1"

  # Freeze globals
  readonly AWS_REGION
  readonly AWS_ACCESS_KEY_ID
  readonly AWS_SECRET_ACCESS_KEY
  readonly RESOURCE_PATH
  readonly AWS_HOST
  readonly AWS_BUCKET_NAME
}

##
# Write error to stderr
# Arguments:
#   $1 string to output
##
err() {
  echo "[$(date +'%Y-%m-%dT%H:%M:%S%z')] Error: $@" >&2
}
info() {
  echo "[$(date +'%Y-%m-%dT%H:%M:%S%z')] Info: $@" >&2
}
main() {
  parseCommandLine $@
  #local get="${PROJECT_PATH}/bin/s3-get"
  put="${PROJECT_PATH}/bin/s3-put"
  export AWS_DEFAULT_REGION=${AWS_REGION}
  export AWS_ACCESS_KEY_ID
  export AWS_SECRET_ACCESS_KEY
  export AWS_BUCKET_NAME
  export AWS_HOST
}

##
# Prevent set -e from triggering an automatic exit
# Eval Get oc logs
# Arguments:
#   $1 oc logs ----
# Returns:
#   oc logs
##

has_log() {
  #echo $1
  if eval "$1"; then
    local logs=$(eval "$1" | grep '.')
    echo $logs
    #echo "shoot ocal " $ys > /dev/stderr
  else
    err "No logs available"
  fi
}

#login
oc_login='oc login --token=$OC_TOKEN --server=$OC_SERVER'
oc_login_sa='oc login --token=$OC_SA_TOKEN'

if [ "$(oc whoami 2>/dev/null | wc -l)" == "0" ]; then
  eval $oc_login || eval $oc_login_sa
fi
#export time must be greater than delayed or sleep time
if [ $EXPORT_TIME -lt $SLEEP_TIME ]; then
  err "EXPORT_TIME must be greater than SLEEP_TIME. pims-logging will gracefully exit after 10s"
  sleep 10
  exit 1
fi

# get openshift logs for container with timestamp for a set sleep time/period
app_oc_logs='oc -n $PROJECT_NAMESPACE logs --timestamps --since=$SLEEP_TIME"s" ${APP_POD_NAME} -c ${APP_CONTAINER_NAME}'
api_oc_logs='oc -n $PROJECT_NAMESPACE logs --timestamps --since=$SLEEP_TIME"s" ${API_POD_NAME} -c ${API_CONTAINER_NAME}'

if [[ $STORAGE_TYPE =~ "Amazon" ]]; then
  app_gzip_curl='zip /logging/${APP_CONTAINER_NAME}_$DATE_NOW.gz /tmp/$FRONTEND_APP_NAME*'
  api_gzip_curl='zip /logging/${API_CONTAINER_NAME}_$DATE_NOW.gz /tmp/$API_NAME*'

  #echo "Upload file to $RESOURCE_PATH"
  app_send_zip='${put} -T "/logging/${APP_CONTAINER_NAME}_${DATE_NOW}.gz" ${APP_LOG_SERVER_URI}'
  api_send_zip='${put} -T "/logging/${API_CONTAINER_NAME}_${DATE_NOW}.gz" ${API_LOG_SERVER_URI}'

  main $@
elif [[ $STORAGE_TYPE =~ "Azure" ]]; then
  # send zipped to endpoint
  app_gzip_curl='zip /logging/${APP_CONTAINER_NAME}_$DATE_NOW.gz /tmp/$FRONTEND_APP_NAME*'
  api_gzip_curl='zip /logging/${API_CONTAINER_NAME}_$DATE_NOW.gz /tmp/$API_NAME*'

  app_send_zip='curl --write-out "%{http_code}\n" -f -X PUT  -H "Content-Type: application/zip" -H "Compression:Gzip" -H "x-ms-date: ${DATE_NOW}" -H "x-ms-version: ${AZ_VERSION}" -H "x-ms-blob-type: BlockBlob" --data-binary "@/logging/${APP_CONTAINER_NAME}_${DATE_NOW}.gz" ${APP_LOG_SERVER_URI}'
  api_send_zip='curl --write-out "%{http_code}\n" -f -X PUT  -H "Content-Type: application/zip" -H "Compression:Gzip" -H "x-ms-date: ${DATE_NOW}" -H "x-ms-version: ${AZ_VERSION}" -H "x-ms-blob-type: BlockBlob" --data-binary "@/logging/${API_CONTAINER_NAME}_${DATE_NOW}.gz" ${API_LOG_SERVER_URI}'

else
  echo "Invalid storage type "
  exit 1
fi

_send() {

  #initialize container variables

  # get app pod and container name
  APP_POD_NAME=$(oc -n $PROJECT_NAMESPACE get pods --selector=name=${FRONTEND_APP_NAME} -o jsonpath="{.items[*].metadata.name}")
  APP_CONTAINER_NAME=$(oc -n $PROJECT_NAMESPACE get pods --selector=name=${FRONTEND_APP_NAME} -o jsonpath={.items[*].spec.containers[*].name})

  # get api pod and container name
  API_POD_NAME=$(oc -n $PROJECT_NAMESPACE get pods --selector=name=${API_NAME} -o jsonpath="{.items[*].metadata.name}")
  API_CONTAINER_NAME=$(oc -n $PROJECT_NAMESPACE get pods --selector=name=${API_NAME} -o jsonpath={.items[*].spec.containers[*].name})

  # set log server URL
  if [[ $STORAGE_TYPE =~ "Azure" ]]; then
    APP_LOG_SERVER_URI="${AZ_BLOB_TARGET}${APP_CONTAINER_NAME}_${logdate}.gz${AZ_SAS_TOKEN}"
    API_LOG_SERVER_URI="${AZ_BLOB_TARGET}${API_CONTAINER_NAME}_${logdate}.gz${AZ_SAS_TOKEN}"
    host_url=$AZ_BLOB_TARGET
  else
    APP_LOG_SERVER_URI="/${AWS_BUCKET_NAME}/${APP_CONTAINER_NAME}_${logdate}.gz"
    API_LOG_SERVER_URI="/${AWS_BUCKET_NAME}/${API_CONTAINER_NAME}_${logdate}.gz"
    host_url="https://"$AWS_HOST/${AWS_BUCKET_NAME}
  fi

  # removes duplicates
  (
    # wait for exclusive lock (fd 200) for 2 seconds
    flock -x 200

    # stores logs temporarily in file
    app_file=/tmp/$APP_POD_NAME'-'$logdate.LOG
    api_file=/tmp/$API_POD_NAME'-'$logdate.LOG

    # grep (-v) select non-matching lines, (-x) that match whole lines, (-f) get patterns from files
    #if log is not empty output to file
    local app_hasLogs=$(has_log "${app_oc_logs}") && [ ! -z "$app_hasLogs" ] && echo "$app_hasLogs" >$app_file
    local api_hasLogs=$(has_log "${api_oc_logs}") && [ ! -z "$api_hasLogs" ] && echo "$api_hasLogs" >$api_file

    #run proess for X# of seconds, check if logs was extracted then upload to server
    if [[ $elapse -ge $EXPORT_TIME && "$(ls -A /tmp/* 2>/dev/null | wc -l)" != "0" && "${TERM_EXIT}" != true ]]; then
      (eval $app_gzip_curl && eval $app_send_zip && rm -f /tmp/$FRONTEND_APP_NAME* && rm -f /logging/$APP_CONTAINER_NAME*) &
      (eval $api_gzip_curl && eval $api_send_zip && rm -f /tmp/$API_NAME* && rm -f /logging/$API_CONTAINER_NAME*)
      if [ "$?" -eq 0 ]; then info "Logs sent to " $host_url; else err "Error sending logs to " $host_url; fi
    elif [[ "$(ls -A /tmp/* 2>/dev/null | wc -l)" == "0" ]]; then
      echo "nothing to zip, logs are empty"
    elif [[ "${TERM_EXIT}" == true ]]; then #check for extracted logs and send before pod scaled to zero
      (eval $app_gzip_curl && eval $app_send_zip && rm -f /tmp/$FRONTEND_APP_NAME* && rm -f /logging/$APP_CONTAINER_NAME*) &
      (eval $api_gzip_curl && eval $api_send_zip && rm -f /tmp/$API_NAME* && rm -f /logging/$API_CONTAINER_NAME*)
      #echo $api_send_zip
      [ "$?" -eq 0 ] && info "Logs sent to " $host_url "via sigterm request"
      [ "$?" -ne 0 ] && err "Error sending logs to " $host_url "via sigterm request"
    else
      echo "waiting for export time to elapse (""$((EXPORT_TIME - elapse))"'s'") remaining"
    fi
  ) 200>/tmp/.data.lock

}

# sends our logs using curl
sendLogs() {
  # first argument is to allow for graceful termination in _term()
  exitSleep=$1
  # we calculate the total processing duration in seconds using bash built-in
  start=${SECONDS}
  # get the datetime for log retrieval. take account of previous duration and sleep
  # format since to be parseable by oc logs - use UTC and no additional timezone info (ex 2020-02-24T18:41:41Z)
  logdate=$(date +"%Y%m%dT%H%M%SZ")
  # debug
  echo "${HOSTNAME}: $(date +'%Y-%m-%d %H:%M:%S') duration: $duration"
  # use openshift client to get logs, gzip them and use curl to send to REST endpoint

  _send
  # this is equivalent to backgrouding our sleep, so we can interrupt when container halted
  sleep ${SLEEP_TIME} &
  wait
  # calculate total processing duration
  duration=$((${SECONDS} - $start))
  #reset elapse time after export time
  [ $elapse -ge $EXPORT_TIME ] && elapse=0
  elapse=$elapse+$duration
}

# sig handler for TERM sent by openshift when pod is stopped or deleted
# need to adjust for graceful termination of the container we are getting logs for
_term() {
  echo "Caught SIGTERM signal! Waiting ${GRACEFUL_EXIT_TIME} seconds before sending"
  TERM_EXIT=true
  sleep 30
  sendLogs ${GRACEFUL_EXIT_TIME} &&
    # exit gracefully
    exit 0
}
trap _term SIGTERM
trap _term SIGINT

# initial startup delay for container we a getting logs for and ourselves
duration=${STARTUP_TIME}
#elapse=$elapse+$duration

# set variables to send logs
while [[ "${API_NAME}" && "${FRONTEND_APP_NAME}" ]]; do
  sendLogs 0
done
# sidecar does nothing if a header is empty
sleep infinity
