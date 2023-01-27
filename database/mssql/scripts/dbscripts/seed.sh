#!/bin/bash

# wait for MSSQL server to start

echo "======= SEEDING DB with test data ======="

#load db schema to local
#Run every scripts in the build folder
for i in "${TARGET_SPRINT}"/Test\ Data/*.sql; do 
   /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $MSSQL_SA_PASSWORD -d pims -i "$i" -b -I 
   count=$?
   if [ $count -ne 0 ]; 
   then echo "======= SCRIPT ${i} RETURNS AN ERROR. ========="
   exit 1;
   else echo "======= SCRIPT ${i} COMPLETED SUCCESSFULLY. =========" && echo $count > /tmp/log.txt
   fi
done

echo "=======DB SEED COMPLETE ========"
