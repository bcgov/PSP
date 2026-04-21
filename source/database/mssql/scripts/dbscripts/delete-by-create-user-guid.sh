#!/bin/bash

# Executes strict safe delete by create user guid for a test user.

eval $(grep -v '^#' ../../.env | xargs)

echo "===== Begin Safe Delete By CreateUserGuid Strict ====="

if [ -z "$TEST_USER_GUID" ];
then
  echo "TEST_USER_GUID not set. Cannot continue";
  exit 1;
fi

sqlcmd_result=$(sqlcmd -S $SERVER_NAME -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -h -1 -Q "SET NOCOUNT ON; EXEC dbo.SafeDeleteByCreateUserGuidStrict @AppCreateUserGuid = '$TEST_USER_GUID', @Execute = 1;")
result=$?

if [ $result -ne 0 ]; then
  echo "Error executing SafeDeleteByCreateUserGuidStrict";
  echo "$sqlcmd_result";
  exit 1;
fi

echo "$sqlcmd_result"
echo "======= Completed Safe Delete By CreateUserGuid Strict ========"