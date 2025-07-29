/* -----------------------------------------------------------------------------
Migrate the data from the TMP_PROP_MGMT_ACTIVITY_SUBTYPE table to the 
PIMS_MGMT_ACTIVITY_SUBTYPE table.
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

-- Migrate the TMP_PROP_MGMT_ACTIVITY_SUBTYPE data to the PIMS_MGMT_ACTIVITY_SUBTYPE table
PRINT N'Migrate the TMP_PROP_MGMT_ACTIVITY_SUBTYPE data to the PIMS_MGMT_ACTIVITY_SUBTYPE table'
GO
INSERT INTO dbo.PIMS_MGMT_ACTIVITY_SUBTYPE (MGMT_ACTIVITY_SUBTYPE_CODE, MGMT_ACTIVITY_TYPE_CODE, DESCRIPTION, IS_DISABLED, DISPLAY_ORDER, CONCURRENCY_CONTROL_NUMBER, DB_CREATE_TIMESTAMP, DB_CREATE_USERID, DB_LAST_UPDATE_TIMESTAMP, DB_LAST_UPDATE_USERID)
  SELECT PROP_MGMT_ACTIVITY_SUBTYPE_CODE
       , PROP_MGMT_ACTIVITY_TYPE_CODE
       , DESCRIPTION
       , IS_DISABLED
       , DISPLAY_ORDER
       , CONCURRENCY_CONTROL_NUMBER
       , DB_CREATE_TIMESTAMP
       , DB_CREATE_USERID
       , DB_LAST_UPDATE_TIMESTAMP
       , DB_LAST_UPDATE_USERID
  FROM   dbo.TMP_PROP_MGMT_ACTIVITY_SUBTYPE
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
