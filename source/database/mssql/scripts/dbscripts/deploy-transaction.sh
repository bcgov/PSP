eval $(grep -v '^#' ../../.env | xargs)
NEWEST_PSP_DIR=$(find ./*PSP* -type d -prune | tail -n 1 | cut -c 3-)
TARGET_SPRINT="${TARGET_SPRINT:-$NEWEST_PSP_DIR}"
echo "* Executing MASTER script in $TARGET_SPRINT against server $SERVER_NAME database name $DB_NAME as user $DB_USER. Continue with Deployment (Y/N)"
read -n 1 -r
if [[ $REPLY =~ ^[Yy]$ ]]
then
   echo
   echo -e "* Enter the name of the target folder to run all db scripts against the database.\n  Valid values are: Alter Up, Alter Down"
   read TARGET_FOLDER
   #load db schema to local
   #Run every scripts in the build folder
   SQLFOLDER="${TARGET_SPRINT}/${TARGET_FOLDER}"
   if [ ! -d "$SQLFOLDER" ]; then
    echo "ERROR: Directory does not exist"
    exit 0
   fi
   i="${SQLFOLDER}"/master.sql;

   # Print the current version of the DB state
   echo " * Current Database version and concurrency value"
   sqlcmd -S $SERVER_NAME -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -Q "SET NOCOUNT ON; SELECT static_variable_value, concurrency_control_number FROM pims_static_variable WHERE static_variable_name = 'DBVERSION';" -W -h -1

   echo "* Executing from '${i}'"
   echo "======= sqlcmd START ========"
   sqlcmd -S $SERVER_NAME -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -i "$i" -b -I
   echo "======= sqlcmd END ========"
   count=$?
   if [ $count -ne 0 ];
     then
       echo "======= SCRIPT ${i} RETURNS AN ERROR. ========="
       exit 1;
    else
      echo "======= SCRIPT ${i} COMPLETED SUCCESSFULLY. =========" && echo $count > /tmp/log.txt
   fi
   # Print the final version of the database after the script execution
   echo " * Final Database version and concurrency value"
   sqlcmd -S $SERVER_NAME -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -Q "SET NOCOUNT ON; SELECT static_variable_value, concurrency_control_number FROM pims_static_variable WHERE static_variable_name = 'DBVERSION';" -W -h -1
fi

echo "======= END ========"

