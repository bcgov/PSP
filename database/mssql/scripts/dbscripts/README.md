# PSP DB Schemas
## Description
The files in this repository are required to build or alter the database on your detination database server. The file types are:
* __README.md:__ The descriptive file that you're reading;
* __*.dez:__ The Dezign file containing the data model associated with the build/alter scripts; and
* __*.zip:__ The zip files containing the scripts and other information that are required to build the database.

The files are named according to the following pattern:
* The first number (following the "S") indicates the sprint number that the scripts were constructed in; and
* The second number (following the ".") indicates the version number of the script within the sprint.

## File Contents

The *.zip files contain the following files:
* **Root Folder**
  * **PIMS PDM Snn.nn.png:** A PNG image of the physical data model intended to provide a visual image of the enclosed database;
  * **PIMS Snn.nn Change Log.txt:** A text file contining the cumulative changes to the database from initial version to the current version;  
* **Folder "Build"**
  * **Note:** The first two-digit number in the file names indicates the ascending order in which the files are to be executed, with the exception of the DROP file, where the file name begins with "nn_".
  * **nn_PSP_PIMS_Snn.nn_Build.sql:** This is the primary build script that, when executed, will build the entire database from scratch.  The first two-digit number in this file name is _usually_ "01", indicating that it is the first file in the Build folder to be executed.
  * **nn_DML_xxxxx.sql:** Is a script that _usually_ populates the code tables with standard values.  There may be many of these scripts in a build.
  * **xx_PSP_PIMS_Snn.nn_DROP.sql:** This script will drop all elements from the database.
* **Folder "Alter Up"**
    * **nn_PSP_PIMS_Snn.nn_Alter.sql:** This script will bring the indicated version in the name up to the current version.  E.g. It would bring version S08.00 up to the S08.01 version.
    * **Note:** If there are multiple scripts in this folder, the scripts should be run in the order indicated by the first two digits in the script name.
* **Folder "Alter Down"**
    * **nn_PSP_PIMS_Snn.nn_Alter.sql:** This script will bring the current version to the previous version.  E.g. It would bring version S08.01 down to the S08.00 version.
    * **Note:** If there are multiple scripts in this folder, the scripts should be run in the order indicated by the first two digits in the script name.

## Execution
Follow the steps below to build or alter the database.

### Build
1. Run the build file (e.g. 01_PSP_PIMS_S09.00_Build.sql)
1. Run any remaining DML scripts in ascending order by file name (e.g. 30_DML_PIMS_ADDRESS_USAGE_TYPE.sql)

### Alter Up
1. Run the alter file (e.g. 01_PSP_PIMS_S08.01_Alter.sql to update the S08.01 database to version S09.00)

### Alter Down
1. Run the alter file (e.g. 01_PSP_PIMS_S09.00_Alter.sql to downgrade the S09.00 database to version S08.01)