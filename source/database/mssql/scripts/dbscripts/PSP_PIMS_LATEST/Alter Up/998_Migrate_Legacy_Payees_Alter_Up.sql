/* -----------------------------------------------------------------------------
Migrate the legacy payees from the PIMS_COMPENSATION_REQUISITION table to the 
PIMS_COMP_REQ_PAYEE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jan-28  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the legacy payee from PIMS_COMPENSATION_REQUISITION to PIMS_COMP_REQ_PAYEE
PRINT N'Migrate the legacy payee from PIMS_COMPENSATION_REQUISITION to PIMS_COMP_REQ_PAYEE'
GO
INSERT INTO PIMS_COMP_REQ_PAYEE (COMPENSATION_REQUISITION_ID, LEGACY_PAYEE, CONCURRENCY_CONTROL_NUMBER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_GUID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_GUID, APP_LAST_UPDATE_USER_DIRECTORY)
  SELECT COMPENSATION_REQUISITION_ID
       , LEGACY_PAYEE
       , 1, getutcdate(), user_name(), newid(), user_name(), getutcdate(), user_name(), newid(), user_name()
  FROM   PIMS_COMPENSATION_REQUISITION
  WHERE  LEGACY_PAYEE IS NOT NULL
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
