eval $(grep -v '^#' ../../.env | xargs)

# Process options
valid=1

# Retrieve the parameters passed
while getopts t:o: option
do
  case "${option}" in
    (t)
      TARGET_SPRINT=$OPTARG
      ;;
    (o)
      TARGET_OPERATION=$OPTARG
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
  exit 0
fi

#load db schema to local
#Run every scripts in the build folder
echo "======= SCRIPT SCHEMA START. ========="
for i in "${TARGET_SPRINT}/${TARGET_OPERATION}"/*.sql; do
   sqlcmd -S $SERVER_NAME -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -i "$i" -b -I
   count=$?
   if [ $count -ne 0 ];
      then echo "======= SCRIPT ${i} RETURNS AN ERROR. ========="
      exit 1;
   else echo "======= SCRIPT ${i} COMPLETED SUCCESSFULLY. =========" && echo $count > /tmp/log.txt
   fi
done
echo "======= DB SCHEMA LOADED ========"

echo "======= END ========"
