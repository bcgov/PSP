eval $(grep -v '^#' ../../.env | xargs)
NEWEST_PSP_DIR=$(find ./*PSP* -type d -prune | tail -n 1 | cut -c 3-)
TARGET_SPRINT="${TARGET_SPRINT:-$NEWEST_PSP_DIR}"
echo "* Executing scripts in $TARGET_SPRINT against server $SERVER_NAME database name $DB_NAME as user $DB_USER. Continue with Deployment (Y/N)"
read -n 1 -r
if [[ $REPLY =~ ^[Yy]$ ]]
then
   echo
   echo -e "* Enter the name of the target folder to run all db scripts against the database.\nValid values are: Alter Up, Alter Down, Build, Drop, Test Data"
   read TARGET_OPERATION

   MASTER_FILE="master.sql";

   directory="${TARGET_SPRINT}/${TARGET_OPERATION}";

   if [ -f "$directory/$MASTER_FILE" ]; then
     echo " * Info: master.sql found. Running transaction..."
     ./db-deploy-transaction.sh -s $TARGET_SPRINT -o "$TARGET_OPERATION"
   else
     echo " * Info: master.sql found. Running transaction..."
     ./db-deploy.sh -s $TARGET_SPRINT -o "$TARGET_OPERATION"
   fi
fi
