eval $(grep -v '^#' ../../.env | xargs)

# Process options
valid=1
dry_run=0

# Retrieve the parameters passed
while getopts s:o:'n' option
do
  case "${option}" in
    (s)
      TARGET_SPRINT=$OPTARG
      ;;
    (o)
      TARGET_OPERATION=$OPTARG
      ;;
    (n)
      dry_run=1
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

#Run every scripts in the build folder
SQLFOLDER="${TARGET_SPRINT}/${TARGET_OPERATION}"
if [ ! -d "$SQLFOLDER" ]; then
 echo "ERROR: Directory does not exist [$SQLFOLDER]"
 exit 1
fi
i="${SQLFOLDER}"/master.sql;

echo "======= DEPLOY-TRANSACTION START ========"

#Print the current version of the DB state
echo " * Current Database version and concurrency value"
sqlcmd -S $SERVER_NAME -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -Q "SET NOCOUNT ON; SELECT static_variable_value, concurrency_control_number FROM pims_static_variable WHERE static_variable_name = 'DBVERSION';" -W -h -1

echo "* Executing from '${i}'"
echo "   ==== sqlcmd START ========"
if [ $dry_run == 0 ]; then
  sqlcmd -S $SERVER_NAME -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -i "$i" -b -I
fi
count=$?
echo "   ==== sqlcmd END ========"
if [ $count -ne 0 ];
  then
    echo "   ==== SCRIPT ${i} RETURNS AN ERROR. ========="
  else
    echo "   ==== SCRIPT ${i} COMPLETED SUCCESSFULLY. =========" && echo $count > /tmp/log.txt
fi

# Print the final version of the database after the script execution
echo " * Final Database version and concurrency value"
sqlcmd -S $SERVER_NAME -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -Q "SET NOCOUNT ON; SELECT static_variable_value, concurrency_control_number FROM pims_static_variable WHERE static_variable_name = 'DBVERSION';" -W -h -1

echo "======= DEPLOY-TRANSACTION END ========"

if [ $count -ne 0 ];
 then
   exit 1
fi
