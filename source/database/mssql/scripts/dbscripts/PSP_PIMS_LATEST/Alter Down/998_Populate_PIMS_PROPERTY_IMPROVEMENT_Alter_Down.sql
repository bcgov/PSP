/* -----------------------------------------------------------------------------
Create a temporary table to manage the transfer of lease improvements to 
property improvements.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Nov-12  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- It the temporary table does not exist, cancel execution of the remainder of 
-- the script.
IF NOT (EXISTS (SELECT object_id
                FROM   sys.tables
                WHERE  name = 'TMP_PROPERTY_IMPROVEMENT'))
SET NOEXEC ON

-- Delete the contents of the PIMS_PROPERTY_IMPROVEMENT table.
PRINT N'Delete the contents of the PIMS_PROPERTY_IMPROVEMENT table.'
GO
DELETE FROM PIMS_PROPERTY_IMPROVEMENT
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Populate PIMS_PROPERTY_IMPROVEMENT from the temporary table.
PRINT N'Populate PIMS_PROPERTY_IMPROVEMENT from the temporary table.'
GO
INSERT INTO dbo.PIMS_PROPERTY_IMPROVEMENT (LEASE_ID, PROPERTY_IMPROVEMENT_TYPE_CODE, STRUCTURE_SIZE, IMPROVEMENT_DESCRIPTION, ADDRESS, CONCURRENCY_CONTROL_NUMBER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_GUID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_GUID, APP_LAST_UPDATE_USER_DIRECTORY, DB_CREATE_TIMESTAMP, DB_CREATE_USERID, DB_LAST_UPDATE_TIMESTAMP, DB_LAST_UPDATE_USERID)
SELECT DISTINCT LEASE_ID
     , PROPERTY_IMPROVEMENT_TYPE_CODE
     , STRUCTURE_SIZE
     , IMPROVEMENT_DESCRIPTION
     , ADDRESS
     , CONCURRENCY_CONTROL_NUMBER
     , APP_CREATE_TIMESTAMP
     , APP_CREATE_USERID
     , APP_CREATE_USER_GUID
     , APP_CREATE_USER_DIRECTORY
     , APP_LAST_UPDATE_TIMESTAMP
     , APP_LAST_UPDATE_USERID
     , APP_LAST_UPDATE_USER_GUID
     , APP_LAST_UPDATE_USER_DIRECTORY
     , DB_CREATE_TIMESTAMP
     , DB_CREATE_USERID
     , DB_LAST_UPDATE_TIMESTAMP
     , DB_LAST_UPDATE_USERID
FROM   TMP_PROPERTY_IMPROVEMENT
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the temporary table.
PRINT N'Drop the temporary table.'
GO
DROP TABLE IF EXISTS dbo.TMP_PROPERTY_IMPROVEMENT
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
IF (@Success = 1) 
  PRINT 'The database update succeeded'
ELSE 
  BEGIN
  IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
    PRINT 'The database update failed'
  END
GO
