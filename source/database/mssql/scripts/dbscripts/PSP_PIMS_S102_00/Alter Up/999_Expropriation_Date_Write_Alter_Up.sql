/* -----------------------------------------------------------------------------
Write the expropriation dates to the PIMS_EXPROP_OWNER_HISTORY table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Mar-21  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Populate PIMS_EXPROP_OWNER_HISTORY from the temp table.
PRINT N'PIMS_EXPROP_OWNER_HISTORY from the temp table.'
GO
INSERT INTO PIMS_EXPROP_OWNER_HISTORY (ACQUISITION_FILE_ID, ACQUISITION_OWNER_ID, INTEREST_HOLDER_ID, EXPROP_OWNER_HISTORY_TYPE_CODE, EVENT_DT)
  SELECT ACQUISITION_FILE_ID
       , ACQUISITION_OWNER_ID
       , INTEREST_HOLDER_ID
       , EXPROP_OWNER_HISTORY_TYPE_CODE
       , EVENT_DT
  FROM   TMP_EXPROP_OWNER_HISTORY;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the temporary table to capture the data.
PRINT N'Drop the temporary table to capture the data.'
GO
DROP TABLE IF EXISTS dbo.TMP_EXPROP_OWNER_HISTORY;
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
