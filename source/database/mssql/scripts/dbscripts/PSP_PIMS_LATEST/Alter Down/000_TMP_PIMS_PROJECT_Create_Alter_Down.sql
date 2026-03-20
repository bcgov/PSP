-- -------------------------------------------------------------------------------------------
-- Collect the region codes of the PIMS_PROJECT_REGION table prior to migration.
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

-- Create the temporary table to capture the data.
PRINT N'Create the temporary table to capture the data.'
GO
CREATE TABLE dbo.TMP_PROJECT (
  ID          bigint   not null,
  REGION_CODE smallint null)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Populate the temp table from the PIMS_PROJECT_REGION table.
PRINT N'Populate the temp table from the PIMS_PROJECT_REGION table.'
GO
INSERT INTO TMP_PROJECT (ID, REGION_CODE)
  SELECT PROJECT_ID
       , REGION_CODE
  FROM   PIMS_PROJECT_REGION;
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
