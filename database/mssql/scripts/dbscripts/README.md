# PSP DB Schemas
## Description
The files in this repository are required to build or alter the database on your detination database server. The file types are:
* __README.md:__ The descriptive file that you're reading;
* __*.txt:__ Text file containing information regarding the database supported supported by this collection of scripts;
* __*.png:__ PNG-format image of the data model associated with this database version; and
* __*.sql:__ SQL script files that are required to build or alter the database.

The files are named according to the following pattern:
* The first number (following the "S") indicates the sprint number that the scripts were developed in; and
* The second number (following the final "_") indicates the version number of the script within the development sprint.

## File Contents

* **Root Folder**
  * **PIMS PDM Snn.nn.png:** A PNG image of the physical data model intended to provide a visual image of the enclosed database;
  * **PIMS Snn.nn Change Log.txt:** A text file contining the cumulative changes to the database from initial version to the current version;
* **Folder "Drop"**
  * **xx_PSP_PIMS_Snn_nn_DROP.sql:** This script will drop all elements from the database.
* **Folder "Build"**
  * **Note:** The first two-digit number in the file names indicates the ascending order in which the files are to be executed, with the exception of the DROP file, where the file name begins with "nn_".
  * **nn_PSP_PIMS_Snn_nn_Build.sql:** This is the primary build script that, when executed, will build the entire database from scratch.  The first two-digit number in this file name is _usually_ "01", indicating that it is the first file in the Build folder to be executed.
  * **nn_DML_xxxxx.sql:** Is a script that _usually_ populates the code tables with standard values.  There may be many of these scripts in a build.
* **Folder "Alter Up"**
    * **nn_PSP_PIMS_Snn_nn_Alter.sql:** This script will bring the indicated version in the name up to the current version.  E.g. It would bring version S08.00 up to the S08.01 version.
    * **Note:** If there are multiple scripts in this folder, the scripts should be run in the order indicated by the first two digits in the script name.
* **Folder "Alter Down"**
    * **nn_PSP_PIMS_Snn_nn_Alter.sql:** This script will bring the current version to the previous version.  E.g. It would bring version S08.01 down to the S08.00 version.
    * **Note:** If there are multiple scripts in this folder, the scripts should be run in the order indicated by the first two digits in the script name.

## Steps to build a Docker container for the database
1. Modify the .env file to contain the following information:
> ACCEPT_EULA=Y  
> MSSQL_SA_PASSWORD=YourPasswordHere <== Create a strong password  
> MSSQL_PID=Developer  
> TZ=America/Los_Angeles  
>  
> TARGET_SPRINT=PSP_PIMS_Snn_nn  <== The target sprint  
> DB_USER=admin  
> DB_PASSWORD=YourPasswordHere  
1. docker build -t pims-database .
1. docker run -p 5433:1433 --name pims --env-file .\.env pims-database  << You may need to change the port number (5433) to avoid a conflict with an existing port number.
