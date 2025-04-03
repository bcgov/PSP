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

-- Populate the PIMS_EXPROPRIATION_NOTICE table from the temp table.
PRINT N'Populate the PIMS_EXPROPRIATION_NOTICE table from the temp table.'
GO
INSERT INTO PIMS_EXPROPRIATION_NOTICE (ACQUISITION_FILE_ID, ACQUISITION_OWNER_ID, EXPROP_NOTICE_SERVED_DT)
  SELECT ACQUISITION_FILE_ID
       , ACQUISITION_OWNER_ID
       , EVENT_DT
  FROM   TMP_EXPROP_OWNER_HISTORY
  WHERE EXPROP_OWNER_HISTORY_TYPE_CODE = N'NOTCSRVDDT';
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Populate the PIMS_EXPROPRIATION_VESTING table from the temp table.
PRINT N'Populate the PIMS_EXPROPRIATION_VESTING table from the temp table.'
GO
INSERT INTO PIMS_EXPROPRIATION_VESTING (ACQUISITION_FILE_ID, EXPROP_VESTING_DT)
  SELECT ACQUISITION_FILE_ID
       , EVENT_DT
  FROM   TMP_EXPROP_OWNER_HISTORY
  WHERE EXPROP_OWNER_HISTORY_TYPE_CODE = N'EXPRVSTNGDT';
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Populate the PIMS_EXPROPRIATION_PAYMENT table from the temp table.
PRINT N'Populate the PIMS_EXPROPRIATION_PAYMENT table from the temp table.'
GO
INSERT INTO PIMS_EXPROPRIATION_PAYMENT (ACQUISITION_FILE_ID, ACQUISITION_OWNER_ID, INTEREST_HOLDER_ID, ADV_PMT_SERVED_DT)
  SELECT ACQUISITION_FILE_ID
       , ACQUISITION_OWNER_ID
       , INTEREST_HOLDER_ID
       , EVENT_DT
  FROM   TMP_EXPROP_OWNER_HISTORY
  WHERE EXPROP_OWNER_HISTORY_TYPE_CODE = N'ADVPMTSRVDDT';
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
