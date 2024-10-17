#!/bin/bash
### Generates a master.sql file that provides trnsactional execution of multiple sql files.
### Options:
###   -f [folder name] | Generate the sql file for the given path. i.e -f 'PSP_PIMS_S88_00/Alter Up'
###   -s [sprint name] | Generate the Alter Up and Alter Down master sql file. i.e -s PSP_PIMS_S88_00

# First parameter is the path
generate_file()
{
  SQLFOLDER=$1
  FILE="${SQLFOLDER}/master.sql"
  SCRIPTHOME=${PWD}

  now_datetime=$(date +'%m/%d/%Y %r')

  echo "Generating master script in for files in '[dbscripts]/$SQLFOLDER'."
  echo " '[dbscripts]/$FILE'."

  if [ ! -d "$SQLFOLDER" ]; then
    echo "ERROR: Directory does not exist"
    exit 0
  fi

  # Initialize the destination file
  cp 'master_script_static/MSSQL_Script_Header.txt' "$FILE"
  echo >> "$FILE"

  echo "-- File generated on ${now_datetime}." | cat - "$FILE" > temp && mv temp "$FILE"


  # Enable extended globbing
  shopt -s extglob nullglob

  # Loop through all SQL files in the current directory
  for sql_file in "$SQLFOLDER"/*.sql; do
    [[ "$sql_file" = *"master.sql" ]] && continue

    echo "   Processing '${sql_file}'..."
    echo "   PRINT '- Executing" $sql_file "'" >> "${FILE}"
    echo '   :r' $sql_file >> "${FILE}"
  done

  # Finalize the destination file
  echo >> "$FILE"
  cat 'master_script_static/MSSQL_Script_Footer.txt' >> "$FILE"

  echo "Generated '[dbscripts]/$FILE'."
}


# Process options
do_path=0
do_sprint=0
valid=1

# Retrieve the parameters passed
while getopts f:s: option
do
  case "${option}" in
    (f)
      do_path=1;
      SQLFOLDER=$OPTARG
      echo "Option file ${OPTARG} was omitted."
      ;;
    (s)
      do_sprint=1;
      TARGET_SPRINT=$OPTARG
      echo "Option sprint ${OPTARG} was omitted."
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

if [ $do_path == 1 ]
then
  generate_file "$SQLFOLDER"
elif [ $do_sprint == 1 ]
then
  generate_file "$TARGET_SPRINT/Alter Up"
  generate_file "$TARGET_SPRINT/Alter Down"
else
  NEWEST_PSP_DIR=$(find ./*PSP* -type d -prune | tail -n 1 | cut -c 3-)
  generate_file "$NEWEST_PSP_DIR/Alter Up"
  generate_file "$NEWEST_PSP_DIR/Alter Down"
fi

