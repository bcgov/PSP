-- -------------------------------------------------------------------------------------------
-- Populate the PIMS_PROJECT table following migration.
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

-- Remove duplicate rows from the TMP_PROJECT table.
PRINT N'Remove duplicate rows from the TMP_PROJECT table.'
GO
WITH CTE AS (
  SELECT *
       , ROW_NUMBER() OVER (PARTITION BY ID ORDER BY (ID)) AS RowRank
  FROM   TMP_PROJECT
)
DELETE 
FROM   CTE
WHERE  RowRank > 1;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
 
-- Populate the PIMS_PROJECT table from the temp table.
PRINT N'Populate the PIMS_PROJECT table from the temp table.'
GO
UPDATE prj
SET    prj.REGION_CODE                = tmp.REGION_CODE
     , prj.CONCURRENCY_CONTROL_NUMBER = prj.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_PROJECT prj JOIN
       TMP_PROJECT  tmp ON tmp.ID = prj.ID
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
