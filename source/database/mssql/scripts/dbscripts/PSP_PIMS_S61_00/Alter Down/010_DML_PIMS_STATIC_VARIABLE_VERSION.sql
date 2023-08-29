/* -----------------------------------------------------------------------------
<<<<<<<< HEAD:source/database/mssql/scripts/dbscripts/PSP_PIMS_S61_00/Alter Down/010_DML_PIMS_STATIC_VARIABLE_VERSION.sql
Update the database version in the PIMS_STATIC_VARIABLE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Nov-24  Initial version
========
Alter the data in the PIMS_PROPERTY_ROAD_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Aug-22  Initial version
>>>>>>>> upstream/test:source/database/mssql/scripts/dbscripts/PSP_PIMS_LATEST/Alter Up/079_DML_PIMS_PROPERTY_ROAD_TYPE_Alter_Up.sql
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

<<<<<<<< HEAD:source/database/mssql/scripts/dbscripts/PSP_PIMS_S61_00/Alter Down/010_DML_PIMS_STATIC_VARIABLE_VERSION.sql
-- Update the database version number.
DECLARE @CurrVer NVARCHAR(100)
SET @CurrVer = N'60.00'

UPDATE PIMS_STATIC_VARIABLE
WITH   (UPDLOCK, SERIALIZABLE) 
SET    STATIC_VARIABLE_VALUE      = @CurrVer
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  STATIC_VARIABLE_NAME       = N'DBVERSION';

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_STATIC_VARIABLE (STATIC_VARIABLE_NAME, STATIC_VARIABLE_VALUE)
    VALUES (N'DBVERSION', @CurrVer);
========
-- Insert the "BYLAW" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'BYLAW'

SELECT PROPERTY_ROAD_TYPE_CODE
FROM   PIMS_PROPERTY_ROAD_TYPE
WHERE  PROPERTY_ROAD_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_PROPERTY_ROAD_TYPE (PROPERTY_ROAD_TYPE_CODE, DESCRIPTION)
    VALUES
      (N'BYLAW', N'Bylaw');
>>>>>>>> upstream/test:source/database/mssql/scripts/dbscripts/PSP_PIMS_LATEST/Alter Up/079_DML_PIMS_PROPERTY_ROAD_TYPE_Alter_Up.sql
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
   IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
   PRINT 'The database update failed'
END
GO
