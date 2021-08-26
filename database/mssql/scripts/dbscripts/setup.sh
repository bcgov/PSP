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

#load db schema to local
#Run every scripts in the build folder
for i in "${TARGET_SPRINT}"/Build/*.sql; do 
   /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $MSSQL_SA_PASSWORD -d $TARGET_SPRINT -i $i
done

#check if schema was loaded successfully
response=$(/opt/mssql-tools/bin/sqlcmd  -S localhost -U sa -P $MSSQL_SA_PASSWORD -d $TARGET_SPRINT -q 'SET NOCOUNT ON; SELECT COUNT(DISTINCT TABLE_NAME) as count FROM information_schema.columns WHERE TABLE_CATALOG = "'${TARGET_SPRINT}'";')
count=$(echo "$response" | grep -o -E '[0-9]+')
if [ "$count" -eq "0" ]; 
then echo "======= ERROR LOADING DB SCHEMA ========"
exit 1;
else echo $response; 
fi

echo "=======DB SCHEMA LOADED ========"


# Wait on the sqlserver process
wait $pid
