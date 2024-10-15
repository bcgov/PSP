#!/bin/bash

# Script to upgrade an existing database to the latest version (default) or TARGET_VERSION (if passed as parameter "TARGET_VERSION=16.01")
# 1. Find current DB version
#    select STATIC_VARIABLE_VALUE from PIMS_STATIC_VARIABLE where STATIC_VARIABLE_NAME = 'dbversion'
# 2. Search for all SQL scripts under the 'Alter Up' folder and order them by folder name and then file name
#    find ./PSP*/'Alter Up' -type f -iname "*.sql" | sort -n
# 3. Execute all SQL scripts that have a higher version than the current DB version and are less or equal to the target version (if target version passed)

eval $(grep -v '^#' ../../.env | xargs)

echo "===== Begin DB Schema Upgrade ====="

# get current db version
currentdbversion=$(sqlcmd -S $SERVER_NAME -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -h -1 -Q "SET NOCOUNT ON; select STATIC_VARIABLE_VALUE from PIMS_STATIC_VARIABLE where STATIC_VARIABLE_NAME = 'dbversion'");

if [ -z "$currentdbversion" ];
then
  echo "Current version installed not found in DB. Cannot upgrade";
  exit 1;
fi

currentdbversion=${currentdbversion:0:5};
echo "current db version installed: ${currentdbversion}";
export currentdbversion;

TARGET_VERSION="${TARGET_VERSION}";
if [ -z "$TARGET_VERSION" ];
then
  echo "TARGET_VERSION: latest";
else
  echo "TARGET_VERSION: ${TARGET_VERSION}";
fi

MASTER_FILE="master.sql";

find ./PSP*/'Alter Up' -type d | sort -n | while read directory; do
  directoryName=${directory##*/};
  scriptversion=${directory:12:5};
  scriptversion=${scriptversion/_/.};
  echo "directory path = $directory";
  echo "directory name = $directoryName";
  echo "directory version = $scriptversion";

  if [ -f "$directory/$MASTER_FILE" ]; then
    echo "$MASTER_FILE exists."
  else
    echo "$MASTER_FILE DOES NOT."
  fi

  for i in "${directory}"/*.sql; do
  echo "file = $i";
  done
  : '
  if awk "BEGIN {exit !($scriptversion > $currentdbversion)}";
	then
    if [ -z "$TARGET_VERSION" ];
    then
      echo "** run sql script: $file";
      sqlcmd -S $SERVER_NAME -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -i "$file" -b -I
      count=$?
      if [ $count -ne 0 ];
        then echo "======= SCRIPT ${file} RETURNS AN ERROR. ========="
        exit 1;
      else echo "======= SCRIPT ${file} COMPLETED SUCCESSFULLY. =========" && echo $count > /tmp/log.txt
      fi
    else
      if awk "BEGIN {exit !($scriptversion <= $TARGET_VERSION)}";
      then
        echo "** run sql script: $file";
        sqlcmd -S $SERVER_NAME -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -i "$file" -b -I
        count=$?
        if [ $count -ne 0 ];
          then echo "======= SCRIPT ${file} RETURNS AN ERROR. ========="
          exit 1;
        else echo "======= SCRIPT ${file} COMPLETED SUCCESSFULLY. =========" && echo $count > /tmp/log.txt
        fi
      fi
    fi
  fi
  '

done

echo "======= Completed DB Schema Upgrade ========"

#if (( $(echo "$scriptversion > $currentdbversion" | bc -l) ));
