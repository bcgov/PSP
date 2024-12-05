/* -----------------------------------------------------------------------------
Alter the data in the PIMS_ACQUISITION_FILE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Nov-14  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Populate FILE_NUM_SUFFIX with the suffix in FILE_NUMBER in PIMS_ACQUISITION_FILE
PRINT N'Populate FILE_NUM_SUFFIX with the suffix in FILE_NUMBER in PIMS_ACQUISITION_FILE'
GO
WITH CTE AS 
  (SELECT FILE_NO
        , FILE_NUMBER
        , FILE_NO_SUFFIX
        , CONCURRENCY_CONTROL_NUMBER
        , ROW_NUMBER() OVER (PARTITION BY FILE_NO ORDER BY FILE_NO, FILE_NUMBER) AS SubRow
   FROM   PIMS_ACQUISITION_FILE)
UPDATE CTE
SET    FILE_NO_SUFFIX = SubRow
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
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
