/* -----------------------------------------------------------------------------
Populate the PIMS_PROP_ACTIVITY_MGMT_ACTIVITY table from the 
TMP_PROPERTY_ACTIVITY temporary table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-May-16  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Populate the PIMS_PROP_ACTIVITY_MGMT_ACTIVITY table from the TMP_PROPERTY_ACTIVITY table.
PRINT N'Populate the PIMS_PROP_ACTIVITY_MGMT_ACTIVITY table from the TMP_PROPERTY_ACTIVITY table.'
GO
INSERT INTO dbo.PIMS_PROP_ACTIVITY_MGMT_ACTIVITY (PIMS_PROPERTY_ACTIVITY_ID, PROP_MGMT_ACTIVITY_SUBTYPE_CODE, CONCURRENCY_CONTROL_NUMBER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_GUID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_GUID, APP_LAST_UPDATE_USER_DIRECTORY)
  SELECT PROPERTY_ACTIVITY_ID
       , PROP_MGMT_ACTIVITY_SUBTYPE_CODE
       , 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'
  FROM   dbo.TMP_PROPERTY_ACTIVITY
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
