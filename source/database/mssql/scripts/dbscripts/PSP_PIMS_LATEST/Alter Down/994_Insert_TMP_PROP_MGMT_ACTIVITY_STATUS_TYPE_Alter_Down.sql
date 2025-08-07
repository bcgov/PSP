/* -----------------------------------------------------------------------------
Migrate the data from the TMP_PROP_MGMT_ACTIVITY_STATUS_TYPE table to the 
PIMS_PROP_MGMT_ACTIVITY_STATUS_TYPE table.
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

-- Migrate the TMP_PROP_MGMT_ACTIVITY_STATUS_TYPE data to the PIMS_PROP_MGMT_ACTIVITY_STATUS_TYPE table
PRINT N'Migrate the TMP_PROP_MGMT_ACTIVITY_STATUS_TYPE data to the PIMS_PROP_MGMT_ACTIVITY_STATUS_TYPE table'
GO
INSERT INTO dbo.PIMS_PROP_MGMT_ACTIVITY_STATUS_TYPE (PROP_MGMT_ACTIVITY_STATUS_TYPE_CODE, DESCRIPTION, IS_DISABLED, DISPLAY_ORDER, CONCURRENCY_CONTROL_NUMBER, DB_CREATE_TIMESTAMP, DB_CREATE_USERID, DB_LAST_UPDATE_TIMESTAMP, DB_LAST_UPDATE_USERID)
  SELECT PROP_MGMT_ACTIVITY_STATUS_TYPE_CODE
       , DESCRIPTION
       , IS_DISABLED
       , DISPLAY_ORDER
       , CONCURRENCY_CONTROL_NUMBER
       , DB_CREATE_TIMESTAMP
       , DB_CREATE_USERID
       , DB_LAST_UPDATE_TIMESTAMP
       , DB_LAST_UPDATE_USERID
  FROM   dbo.TMP_PROP_MGMT_ACTIVITY_STATUS_TYPE
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
