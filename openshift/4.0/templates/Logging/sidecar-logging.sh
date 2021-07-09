#!/usr/bin/env bash

# debug on fail
set -euo pipefail

# sensible defaults if not set
: "${DEDUPE:=true}"
: "${TERM_EXIT:=false}"
: "${SLEEP_TIME:=60}"
: "${EXPORT_TIME:=120}"
: "${GRACEFUL_EXIT_TIME:=120}"
: "${HOSTNAME:=}"
: "${STARTUP_TIME:=0}"
declare -i elapse=0


DATE_NOW=$(date +"%Y%m%d")
AZ_VERSION="2020-04-08"
AZ_BLOB_TARGET="${AZ_BLOB_URL}/${AZ_BLOB_CONTAINER}/"

#login
oc_login='oc login --token=$OC_TOKEN --server=$OC_SERVER'

  if [ "$(oc whoami 2>/dev/null | wc -l)" == "0" ]; then
    eval $oc_login; 
  fi
#export time must be greater than delayed or sleep time
if [ $EXPORT_TIME -lt $SLEEP_TIME ]; then
	echo "EXPORT_TIME must be greater than SLEEP_TIME. pims-logging will gracefully exit after 10s"
	sleep 10
	exit 0
fi

# get app pod and container name
APP_POD_NAME=$(oc -n $PROJECT_NAMESPACE get pods --selector=name=${FRONTEND_APP_NAME} -o   jsonpath="{.items[*].metadata.name}")
APP_CONTAINER_NAME=$(oc -n $PROJECT_NAMESPACE get pods --selector=name=${FRONTEND_APP_NAME} -o jsonpath={.items[*].spec.containers[*].name})

# get api pod and container name
API_POD_NAME=$(oc -n $PROJECT_NAMESPACE get pods --selector=name=${API_NAME} -o   jsonpath="{.items[*].metadata.name}")
API_CONTAINER_NAME=$(oc -n $PROJECT_NAMESPACE get pods --selector=name=${API_NAME} -o jsonpath={.items[*].spec.containers[*].name})

# set log server URL
APP_LOG_SERVER_URI="${AZ_BLOB_TARGET}${APP_CONTAINER_NAME}_${DATE_NOW}.gz${AZ_SAS_TOKEN}"
API_LOG_SERVER_URI="${AZ_BLOB_TARGET}${API_CONTAINER_NAME}_${DATE_NOW}.gz${AZ_SAS_TOKEN}"


# get openshift logs for container with timestamp for a set sleep time/period
app_oc_logs='oc -n $PROJECT_NAMESPACE logs --timestamps --since=$SLEEP_TIME"s" ${APP_POD_NAME} -c ${APP_CONTAINER_NAME}'
api_oc_logs='oc -n $PROJECT_NAMESPACE logs --timestamps --since=$SLEEP_TIME"s" ${API_POD_NAME} -c ${API_CONTAINER_NAME}'

# send zipped to endpoint 
app_gzip_curl='zip /logging/${APP_CONTAINER_NAME}_$DATE_NOW.gz /tmp/$APP_POD_NAME*'
api_gzip_curl='zip /logging/${API_CONTAINER_NAME}_$DATE_NOW.gz /tmp/$API_POD_NAME*'
app_send_zip='curl -v -X PUT  -H "Content-Type: application/zip" -H "Compression:Gzip" -H "x-ms-date: ${DATE_NOW}" -H "x-ms-version: ${AZ_VERSION}" -H "x-ms-blob-type: BlockBlob" --data-binary "@/logging/${APP_CONTAINER_NAME}_${DATE_NOW}.gz" ${APP_LOG_SERVER_URI}'
api_send_zip='curl -v -X PUT  -H "Content-Type: application/zip" -H "Compression:Gzip" -H "x-ms-date: ${DATE_NOW}" -H "x-ms-version: ${AZ_VERSION}" -H "x-ms-blob-type: BlockBlob" --data-binary "@/logging/${API_CONTAINER_NAME}_${DATE_NOW}.gz" ${API_LOG_SERVER_URI}'




_send() {


  # removes duplicates
  (
    # wait for exclusive lock (fd 200) for 2 seconds
    flock -x 200

    # stores logs temporarily in file
    app_file=/tmp/$APP_POD_NAME'-'$logdate.LOG ; api_file=/tmp/$API_POD_NAME'-'$logdate.LOG

    # grep (-v) select non-matching lines, (-x) that match whole lines, (-f) get patterns from files 
	#if log is not empty output to file
 
	(app_hasLogs=$(eval $app_oc_logs | grep '.') && [ ! -z "$app_hasLogs" ] && echo "$app_hasLogs" > $app_file) & (api_hasLogs=$(eval $api_oc_logs | grep '.') && [ ! -z "$api_hasLogs" ] && echo "$api_hasLogs" > $api_file)

  #run proess for X# of seconds, check if logs was extracted then upload to server
	if [[ $elapse -ge $EXPORT_TIME && "$(ls -A /tmp/*$DATE_NOW* 2>/dev/null | wc -l)" != "0" && "${TERM_EXIT}" != true ]]
	then
        APP_LOG_SERVER_URI="${AZ_BLOB_TARGET}${APP_CONTAINER_NAME}_${logdate}.gz${AZ_SAS_TOKEN}"
        API_LOG_SERVER_URI="${AZ_BLOB_TARGET}${API_CONTAINER_NAME}_${logdate}.gz${AZ_SAS_TOKEN}"
		    (eval $app_gzip_curl && eval $app_send_zip && rm -f /tmp/$APP_POD_NAME* && rm -f /logging/$APP_CONTAINER_NAME*) & (eval $api_gzip_curl && eval $api_send_zip && rm -f /tmp/$API_POD_NAME* && rm -f /logging/$API_CONTAINER_NAME*)
        #clear elapse time
   elif [[ "$(ls -A /tmp/*$DATE_NOW* 2>/dev/null | wc -l)" == "0" ]]
   then
      echo "nothing to zip, logs are empty"
   elif [[ "${TERM_EXIT}" == true ]] #check for extracted logs and send before pod scaled to zero
   then
		  APP_LOG_SERVER_URI="${AZ_BLOB_TARGET}${APP_CONTAINER_NAME}_${logdate}.gz${AZ_SAS_TOKEN}"
      API_LOG_SERVER_URI="${AZ_BLOB_TARGET}${API_CONTAINER_NAME}_${logdate}.gz${AZ_SAS_TOKEN}"
		  (eval $app_gzip_curl && eval $app_send_zip && rm -f /tmp/$APP_POD_NAME* && rm -f /logging/$APP_CONTAINER_NAME*) & (eval $api_gzip_curl && eval $api_send_zip && rm -f /tmp/$API_POD_NAME* && rm -f /logging/$API_CONTAINER_NAME*)
      echo "Logs sent on sigterm request"
    else 
        echo "waiting for export time to elapse (""$((EXPORT_TIME-elapse))"'s'") remaining"
	fi
  ) 200>/tmp/.data.lock

};

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
  echo "${HOSTNAME}: $(date +'%Y-%m-%d %H:%M:%S' | sed 's/\(:[0-9][0-9]\)[0-9]*$/\1/') duration: $duration"
  # use openshift client to get logs, gzip them and use curl to send to REST endpoint
  _send
  # this is equivalent to backgrouding our sleep, so we can interrupt when container halted
  sleep ${SLEEP_TIME} &
  wait 
  # calculate total processing duration
  duration=$((${SECONDS} - $start));
  #reset elapse time after export time
  [ $elapse -ge $EXPORT_TIME ] && elapse=0
  elapse=$elapse+$duration
};

# sig handler for TERM sent by openshift when pod is stopped or deleted
# need to adjust for graceful termination of the container we are getting logs for
_term() {
  echo "Caught SIGTERM signal! Waiting ${GRACEFUL_EXIT_TIME} seconds before sending"
  TERM_EXIT=true
  #sleep ${GRACEFUL_EXIT_TIME} 
  sendLogs ${GRACEFUL_EXIT_TIME} && echo "Logs sent on sigterm request"
  # exit gracefully
  exit 0
}
trap _term SIGTERM

# initial startup delay for container we a getting logs for and ourselves
duration=${STARTUP_TIME};
#elapse=$elapse+$duration


# set variables to send logs
while [[ "${APP_POD_NAME}" && "${API_POD_NAME}" && "${APP_LOG_SERVER_URI}" ]];
  do sendLogs 0
done
# sidecar does nothing if a header is empty
sleep infinity
