#!/bin/bash

# Retrieve the parameters passed
while getopts f:s: option
do 
  case "${option}"
      in
      f)FOLDER=${OPTARG};;
      s)SUBFOLDER=${OPTARG};;
  esac
done

# Set your output file
SQLFOLDER=${FOLDER}${SUBFOLDER}
FILE=${FOLDER}${SUBFOLDER}'\master.sql'
SCRIPTHOME=${PWD}

echo $SQLFOLDER
echo $FILE
echo $SCRIPTHOME

# Initialize the destination file
cp 'MSSQL_Script_Header.txt' "$FILE"
echo >> "$FILE"

# exit 0

cd "$SQLFOLDER"

# Enable extended globbing
shopt -s extglob nullglob

# Loop through all SQL files in the current directory
for sql_file in !(master).sql
do
  echo 'Running $sql_file...' $sql_file
  echo "PRINT 'Executing" $sql_file "'" >> "$FILE"
  echo ':r' $sql_file >> "$FILE"
done

cd $SCRIPTHOME

# Finalize the destination file
echo >> "$FILE"
cat 'MSSQL_Script_Footer.txt' >> "$FILE"
