#!/bin/bash

# Script to upgrade an existing database to the latest version (default) or TARGET_VERSION (if passed as parameter "TARGET_VERSION=16.01")
# 1. Find current DB version
#    select STATIC_VARIABLE_VALUE from PIMS_STATIC_VARIABLE where STATIC_VARIABLE_NAME = 'dbversion'
# 2. Search for all SQL scripts under the 'Alter Up' folder and order them by folder name and then file name
#    find ./PSP*/'Alter Up' -type f -iname "*.sql" | sort -n
# 3. Execute all SQL scripts that have a higher version than the current DB version and are less or equal to the target version (if target version passed)

eval $(grep -v '^#' ../../.env | xargs)


echo "===== Begin DB Schema Upgrade ====="
get_current_db_version()
{
  currentdbversion=$(sqlcmd -S $SERVER_NAME -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -h -1 -Q "SET NOCOUNT ON; select STATIC_VARIABLE_VALUE from PIMS_STATIC_VARIABLE where STATIC_VARIABLE_NAME = 'dbversion'");
  result=$?
  if [ $result -ne 0 ]; then
    echo "Error getting DB version";
    echo $currentdbversion;
    exit 1;
  fi

  if [ -z "$currentdbversion" ];
  then
    echo "Current version installed not found in DB. Cannot upgrade";
    exit 1;
  fi

  echo $currentdbversion;

  currentdbversion=${currentdbversion:0:5};
}

get_current_db_version
initialdbversion=$currentdbversion

if [ -z "$currentdbversion" ];
then
  echo "Current version installed not found in DB. Cannot upgrade";
  exit 1;
fi

echo "current db version installed: ${currentdbversion}";

NEWEST_PSP_DIR=$(find ./*PSP* -type d -prune | tail -n 1 | cut -c 3-)
if [ -z "$TARGET_SPRINT" ];
then
  echo "TARGET_VERSION: latest";
  TARGET_VERSION="${NEWEST_PSP_DIR}"
else
  echo "TARGET_VERSION: ${TARGET_VERSION}";
  TARGET_VERSION="${TARGET_SPRINT}"
fi

MASTER_FILE="master.sql";

find ./PSP*/'Alter Up' -type d | sort -n | while read directory; do
  TARGET_OPERATION=${directory##*/};
  TARGET_SPRINT=${directory#*/};
  TARGET_SPRINT=${TARGET_SPRINT%'/Alter Up'};
  echo "directory path = $directory";

  # only process directories that are a migration
  if [[ $TARGET_SPRINT == "PSP_PIMS_S"* ]]; then
    echo "target_operation = $TARGET_OPERATION";
    echo "target_sprint = $TARGET_SPRINT";

    directoryVersion=${directory:12:5};
    scriptversion=${directoryVersion/_/.};
    echo "directory version = $scriptversion";

    #get_current_db_version
    if awk "BEGIN {exit !($scriptversion > $initialdbversion)}"; then
      if awk "BEGIN {exit !($scriptversion <= $TARGET_VERSION)}"; then
        if [ -f "$directory/$MASTER_FILE" ]; then
          echo "$MASTER_FILE exists."
          #' call transaction deploy'
          #./db-deploy-transaction.sh -t $TARGET_SPRINT -o 'Alter Up'
        else
          echo "$MASTER_FILE DOES NOT."
          #' call regular deploy'
          #./db-deploy.sh -t $TARGET_SPRINT -o 'Alter Up'
        fi
      else
        echo "Too OLD"
      fi
    else
      echo "Too YOUNG"
    fi
  else
    echo "Not Deployable"
  fi
done

echo "======= Completed DB Schema Upgrade ========"
