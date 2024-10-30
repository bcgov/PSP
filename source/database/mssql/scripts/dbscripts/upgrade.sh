#!/bin/bash

# Script to upgrade an existing database to the latest version (default) or TARGET_VERSION (if passed as parameter "TARGET_VERSION=16.01")
# 1. Find current DB version
#    select STATIC_VARIABLE_VALUE from PIMS_STATIC_VARIABLE where STATIC_VARIABLE_NAME = 'dbversion'
# 2. Search for all SQL scripts under the 'Alter Up' folder and order them by folder name and then file name
#    find ./PSP*/'Alter Up' -type f -iname "*.sql" | sort -n
# 3. Execute all SQL scripts that have a higher version than the current DB version and are less or equal to the target version (if target version passed)

eval $(grep -v '^#' ../../.env | xargs)

# Process options
valid=1
dry_run=0

# Retrieve the parameters passed
while getopts n: option
do
  case "${option}" in
    (n)
      dry_run=$OPTARG
      ;;

    # Option error handling.
    \?) valid=0
      echo "An invalid option has been entered: $OPTARG"
      ;;

    :)  valid=0
      echo "The additional argument for option $OPTARG was omitted."
      ;;
  esac
done

if [ $valid == 0 ]
then
  exit 1
fi

if [ $dry_run == 1 ]
then
  echo "** Running DRY RUN **"
fi

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

  currentdbversion=${currentdbversion:0:5};
}

get_current_db_version
initialdbversion=$currentdbversion

if [ -z "$currentdbversion" ];
then
  echo "Current version installed not found in DB. Cannot upgrade";
  exit 1;
fi

echo "Current db version installed: ${currentdbversion}";

NEWEST_PSP_DIR=$(find ./*PSP* -type d -prune | tail -n 1 | cut -c 3-)
if [ -z "$TARGET_SPRINT" ];
then
  echo "Targeting Latest";
  TARGET_SPRINT="${NEWEST_PSP_DIR}"
else
  echo "Targeting ${TARGET_SPRINT}";
fi

TARGET_VERSION=${TARGET_SPRINT:10:5};
TARGET_VERSION=${TARGET_VERSION/_/.};
echo "TARGET_VERSION: ${TARGET_VERSION}";

MASTER_FILE="master.sql";

find ./PSP*/'Alter Up' -type d | sort -n | while read directory; do
  MIGRATION_OPERATION=${directory##*/};
  MIGRATION_FOLDER=${directory#*/};
  MIGRATION_FOLDER=${MIGRATION_FOLDER%'/Alter Up'};
  #echo "directory path = $directory";
  #echo "migration folder = $MIGRATION_FOLDER";

  # only process directories that are a migration
  if [[ $MIGRATION_FOLDER == "PSP_PIMS_S"* ]]; then
    #echo "target_operation = $MIGRATION_OPERATION";
    #echo "migration folder = $MIGRATION_FOLDER";

    directoryVersion=${directory:12:5};
    scriptversion=${directoryVersion/_/.};
    #echo "directory version = $scriptversion";

    if awk "BEGIN {exit !($scriptversion > $initialdbversion)}"; then
      if awk "BEGIN {exit !($scriptversion <= $TARGET_VERSION)}"; then
        if [ -f "$directory/$MASTER_FILE" ]; then
          echo " * Info: master.sql found. Running transaction..."
          #' call transaction deploy'
          if [ $dry_run == 0 ]; then
            ./db-deploy-transaction.sh -s $MIGRATION_FOLDER -o 'Alter Up'
          else
            ./db-deploy-transaction.sh -s $MIGRATION_FOLDER -o 'Alter Up' -n
          fi
        else
          echo " * Info: master.sql NOT found. Running incremental..."
          #' call regular deploy'
          if [ $dry_run == 0 ]; then
            ./db-deploy.sh -s $MIGRATION_FOLDER -o 'Alter Up'
          else
            ./db-deploy.sh -s $MIGRATION_FOLDER -o 'Alter Up' -n
          fi
        fi
      #else echo "Version ABOVE target, skipping"
      fi
    #else echo "Version BELOW current, skipping"
    fi
  #else echo "Folder not a migration"
  fi
done

echo "======= Completed DB Schema Upgrade ========"
