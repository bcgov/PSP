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
    n)
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

#Run every script in the build folder
echo "======= DEPLOY START. ========="
for i in "${TARGET_SPRINT}/${TARGET_OPERATION}"/*.sql; do
   if [ $dry_run == 0 ]; then
      sqlcmd -S $SERVER_NAME -U $DB_USER -P $DB_PASSWORD -d $DB_NAME -i "$i" -b -I
   fi
   count=$?
   if [ $count -ne 0 ]; then
      echo "   ==== SCRIPT ${i} RETURNS AN ERROR. ========="
      exit 1;
   else
      echo "   ==== SCRIPT ${i} COMPLETED SUCCESSFULLY. =========" && echo $count > /tmp/log.txt
   fi
done
echo "   ==== DB SCHEMA LOADED ========"

echo "======= DEPLOY END ========"
