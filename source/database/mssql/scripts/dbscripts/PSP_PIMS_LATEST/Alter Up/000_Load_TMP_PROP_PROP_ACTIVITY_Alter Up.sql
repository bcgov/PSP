/* -----------------------------------------------------------------------------
Create and populate the temporary TMP_PROP_PROP_ACTIVITY table.  This data will
be migrated to the PIMS_MANAGEMENT_ACTIVITY_PROPERTY table in IS-107.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Jul-07  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the temporary table
PRINT N'Drop the temporary table'
GO
DROP TABLE IF EXISTS dbo.TMP_PROP_PROP_ACTIVITY
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table
PRINT N'Create the temporary table'
GO
CREATE TABLE dbo.TMP_PROP_PROP_ACTIVITY (
  PROP_PROP_ACTIVITY_ID          bigint,
  PROPERTY_ID                    bigint,
  PIMS_PROPERTY_ACTIVITY_ID      bigint,
  CONCURRENCY_CONTROL_NUMBER     bigint,
  APP_CREATE_TIMESTAMP           datetime,
  APP_CREATE_USERID              nvarchar(30),
  APP_CREATE_USER_GUID           uniqueidentifier,
  APP_CREATE_USER_DIRECTORY      nvarchar(30),
  APP_LAST_UPDATE_TIMESTAMP      datetime,
  APP_LAST_UPDATE_USERID         nvarchar(30),
  APP_LAST_UPDATE_USER_GUID      uniqueidentifier,
  APP_LAST_UPDATE_USER_DIRECTORY nvarchar(30),
  DB_CREATE_TIMESTAMP            datetime,
  DB_CREATE_USERID               nvarchar(30),
  DB_LAST_UPDATE_TIMESTAMP       datetime,
  DB_LAST_UPDATE_USERID          nvarchar(30)
  )
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the PIMS_PROP_PROP_ACTIVITY data to the TMP_PROP_PROP_ACTIVITY table
PRINT N'Migrate the PIMS_PROP_PROP_ACTIVITY data to the TMP_PROP_PROP_ACTIVITY table'
GO
INSERT INTO dbo.TMP_PROP_PROP_ACTIVITY (PROP_PROP_ACTIVITY_ID, PROPERTY_ID, PIMS_PROPERTY_ACTIVITY_ID, CONCURRENCY_CONTROL_NUMBER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_GUID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_GUID, APP_LAST_UPDATE_USER_DIRECTORY, DB_CREATE_TIMESTAMP, DB_CREATE_USERID, DB_LAST_UPDATE_TIMESTAMP, DB_LAST_UPDATE_USERID)
  SELECT PROP_PROP_ACTIVITY_ID
       , PROPERTY_ID
       , PIMS_PROPERTY_ACTIVITY_ID
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
  FROM   dbo.PIMS_PROP_PROP_ACTIVITY
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
