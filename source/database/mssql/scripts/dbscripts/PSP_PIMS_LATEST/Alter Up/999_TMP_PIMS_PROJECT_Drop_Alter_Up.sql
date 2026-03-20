-- -------------------------------------------------------------------------------------------
-- Populate the PIMS_PROJECT_REGION table following migration.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2026-Mar-17  PSP-11252   Multi-select Option for MoTI Region Field.
-- -------------------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Populate PIMS_PROJECT_REGION from the temp table.
PRINT N'Populate PIMS_PROJECT_REGION from the temp table.'
GO
INSERT INTO PIMS_PROJECT_REGION (PROJECT_ID, REGION_CODE, CONCURRENCY_CONTROL_NUMBER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_DIRECTORY)
  SELECT ID
       , REGION_CODE
       , 1, getutcdate(), N'PSP-11276', N'PSP-11276', getutcdate(), N'PSP-11276', N'PSP-11276'
  FROM   TMP_PROJECT;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the temporary table.
PRINT N'Drop the temporary table.'
GO
DROP TABLE IF EXISTS dbo.TMP_PROJECT;
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
