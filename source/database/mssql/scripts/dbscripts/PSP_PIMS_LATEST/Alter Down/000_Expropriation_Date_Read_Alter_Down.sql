/* -----------------------------------------------------------------------------
Collect the expropriation dates prior to migration to the 
PIMS_EXPROP_OWNER_HISTORY table.
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

-- Populate the temp table from the dbo.PIMS_EXPROP_OWNER_HISTORY table.
PRINT N'Populate the temp table from the dbo.PIMS_EXPROP_OWNER_HISTORY table.'
GO
INSERT INTO TMP_EXPROP_OWNER_HISTORY (ACQUISITION_FILE_ID, ACQUISITION_OWNER_ID, INTEREST_HOLDER_ID, EXPROP_OWNER_HISTORY_TYPE_CODE, EVENT_DT)
  SELECT ACQUISITION_FILE_ID
       , ACQUISITION_OWNER_ID
       , INTEREST_HOLDER_ID
       , EXPROP_OWNER_HISTORY_TYPE_CODE
       , EVENT_DT
  FROM   dbo.PIMS_EXPROP_OWNER_HISTORY
  WHERE EXPROP_OWNER_HISTORY_TYPE_CODE <> N'CERTEXPRAPPR';
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
