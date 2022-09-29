#!/bin/bash

# wait for MSSQL server to start
pid=$!

echo "Waiting for MS SQL to be available ⏳"
/opt/mssql-tools/bin/sqlcmd -l 30 -S localhost -h-1 -V1 -U sa -P $MSSQL_SA_PASSWORD -Q "SET NOCOUNT ON SELECT \"YAY WE ARE UP\" , @@servername"
is_up=$?
while [ $is_up -ne 0 ] ; do
	echo -e $(date)
	/opt/mssql-tools/bin/sqlcmd -l 30 -S localhost -h-1 -V1 -U sa -P $MSSQL_SA_PASSWORD -Q "SET NOCOUNT ON SELECT \"YAY WE ARE UP\" , @@servername"
	is_up=$?
	sleep 5
done

echo "======= MSSQL SERVER STARTED ========"

# Run the setup script to create the DB and the schema in the DB
/opt/mssql-tools/bin/sqlcmd -l 30 -S localhost -U sa -P $MSSQL_SA_PASSWORD -d master -i init.sql

echo "======= MSSQL CONFIG COMPLETE ======="

NEWEST_PSP_DIR=$(find ./*PSP* -type d -prune | tail -n 1 | cut -c 3-)
TARGET_SPRINT="${TARGET_SPRINT:-$NEWEST_PSP_DIR}"
echo "Executing SQL scripts in $TARGET_SPRINT"
#load db schema to local
#Run every scripts in the build folder
for i in "${TARGET_SPRINT}"/Build/*.sql; do 
   /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $MSSQL_SA_PASSWORD -d pims -i $i -b -I 
   count=$?
   if [ $count -ne 0 ]; 
   then echo "======= SCRIPT ${i} RETURNS AN ERROR. ========="
   exit 1;
   else echo "======= SCRIPT ${i} COMPLETED SUCCESSFULLY. =========" && echo $count > /tmp/log.txt
   fi
done

echo "=======DB SCHEMA LOADED ========"

if [ "$SEED" ] ; then ./seed.sh; fi;


# Wait on the sqlserver process
wait $pid
