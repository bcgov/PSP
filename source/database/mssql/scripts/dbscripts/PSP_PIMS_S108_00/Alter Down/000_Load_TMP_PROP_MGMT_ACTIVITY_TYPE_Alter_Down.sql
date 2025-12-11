/* -----------------------------------------------------------------------------
Create and populate the temporary TMP_PROP_MGMT_ACTIVITY_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Jul-24  Initial version
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
DROP TABLE IF EXISTS dbo.TMP_PROP_MGMT_ACTIVITY_TYPE
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table
PRINT N'Create the temporary table'
GO
CREATE TABLE dbo.TMP_PROP_MGMT_ACTIVITY_TYPE (
  PROP_MGMT_ACTIVITY_TYPE_CODE nvarchar(20),
  DESCRIPTION                  nvarchar(200),
  IS_DISABLED                  bit,
  DISPLAY_ORDER                int,
	CONCURRENCY_CONTROL_NUMBER   bigint,
	DB_CREATE_TIMESTAMP          datetime,
	DB_CREATE_USERID             nvarchar(30),
	DB_LAST_UPDATE_TIMESTAMP     datetime,
	DB_LAST_UPDATE_USERID        nvarchar(30)
  )
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the PIMS_MGMT_ACTIVITY_TYPE data to the TMP_PROP_MGMT_ACTIVITY_TYPE table
PRINT N'Migrate the PIMS_MGMT_ACTIVITY_TYPE data to the TMP_PROP_MGMT_ACTIVITY_TYPE table'
GO
INSERT INTO dbo.TMP_PROP_MGMT_ACTIVITY_TYPE (PROP_MGMT_ACTIVITY_TYPE_CODE, DESCRIPTION, IS_DISABLED, DISPLAY_ORDER, CONCURRENCY_CONTROL_NUMBER, DB_CREATE_TIMESTAMP, DB_CREATE_USERID, DB_LAST_UPDATE_TIMESTAMP, DB_LAST_UPDATE_USERID)
  SELECT MGMT_ACTIVITY_TYPE_CODE
       , DESCRIPTION
       , IS_DISABLED
       , DISPLAY_ORDER
       , CONCURRENCY_CONTROL_NUMBER
       , DB_CREATE_TIMESTAMP
       , DB_CREATE_USERID
       , DB_LAST_UPDATE_TIMESTAMP
       , DB_LAST_UPDATE_USERID
  FROM   dbo.PIMS_MGMT_ACTIVITY_TYPE
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
