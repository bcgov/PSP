eval $(grep -v '^#' ../../.env | xargs)
NEWEST_PSP_DIR=$(find ./*PSP* -type d -prune | tail -n 1 | cut -c 3-)
TARGET_SPRINT="${TARGET_SPRINT:-$NEWEST_PSP_DIR}"
echo "* Executing scripts in $TARGET_SPRINT against server $SERVER_NAME database name $DB_NAME as user $DB_USER. Continue with Deployment (Y/N)"
read -n 1 -r
if [[ $REPLY =~ ^[Yy]$ ]]
then
   echo
   echo -e "* Enter the name of the target folder to run all db scripts against the database.\nValid values are: Alter Up, Alter Down, Build, Drop, Test Data"
   read TARGET_FOLDER
   ./db-deploy.sh -t $TARGET_SPRINT -o $TARGET_FOLDER
fi

echo "======= END ========"
