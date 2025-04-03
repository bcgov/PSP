/* -----------------------------------------------------------------------------
Collect the expropriation dates prior to migration to the 
PIMS_EXPROP_OWNER_HISTORY table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Mar-21  Initial version.
Doug Filteau  2025-Apr-03  Changed for reconfigured PIMS_EXPROP_OWNER_HISTORY.
----------------------------------------------------------------------------- */

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
CREATE TABLE dbo.TMP_EXPROP_OWNER_HISTORY (
  ACQUISITION_FILE_ID            bigint       not null,
  ACQUISITION_OWNER_ID           bigint       null,
  INTEREST_HOLDER_ID             bigint       null,
  EXPROP_OWNER_HISTORY_TYPE_CODE nvarchar(20) not null,
  EVENT_DT                       datetime
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Populate the temp table from the PIMS_EXPROPRIATION_NOTICE table.
PRINT N'Populate the temp table from the PIMS_EXPROPRIATION_NOTICE table.'
GO
INSERT INTO TMP_EXPROP_OWNER_HISTORY (ACQUISITION_FILE_ID, ACQUISITION_OWNER_ID, EXPROP_OWNER_HISTORY_TYPE_CODE, EVENT_DT)
  SELECT ACQUISITION_FILE_ID
       , ACQUISITION_OWNER_ID
       , N'NOTCSRVDDT'
       , EXPROP_NOTICE_SERVED_DT
  FROM   PIMS_EXPROPRIATION_NOTICE
  WHERE ACQUISITION_OWNER_ID IS NOT NULL;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Populate the temp table from the PIMS_EXPROPRIATION_VESTING table.
PRINT N'Populate the temp table from the PIMS_EXPROPRIATION_VESTING table.'
GO
INSERT INTO TMP_EXPROP_OWNER_HISTORY (ACQUISITION_FILE_ID, EXPROP_OWNER_HISTORY_TYPE_CODE, EVENT_DT)
  SELECT ACQUISITION_FILE_ID
       , N'EXPRVSTNGDT'
       , EXPROP_VESTING_DT
  FROM   PIMS_EXPROPRIATION_VESTING
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Populate the temp table from the PIMS_EXPROPRIATION_PAYMENT table.
PRINT N'Populate the temp table from the PIMS_EXPROPRIATION_PAYMENT table.'
GO
INSERT INTO TMP_EXPROP_OWNER_HISTORY (ACQUISITION_FILE_ID, ACQUISITION_OWNER_ID, INTEREST_HOLDER_ID, EXPROP_OWNER_HISTORY_TYPE_CODE, EVENT_DT)
  SELECT ACQUISITION_FILE_ID
       , ACQUISITION_OWNER_ID
       , INTEREST_HOLDER_ID
       , N'ADVPMTSRVDDT'
       , ADV_PMT_SERVED_DT
  FROM   PIMS_EXPROPRIATION_PAYMENT
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
