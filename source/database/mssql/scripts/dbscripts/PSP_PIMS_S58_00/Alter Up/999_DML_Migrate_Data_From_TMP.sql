/* -----------------------------------------------------------------------------
Migrate the data to the PIMS_INTEREST_HOLDER and PIMS_ACQUISITION_PAYEE tables 
from the temporary tables.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jul-06  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table for the transitional interest holders
DROP TABLE IF EXISTS [dbo].[TMP_INTEREST_HOLDER_TXN] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table for the transitional interest holders
CREATE TABLE [dbo].[TMP_INTEREST_HOLDER_TXN] (
    [NEW_AO_REP_SOL_ID]    BIGINT,
    [LEGACY_AO_REP_SOL_ID] BIGINT
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_INTEREST_HOLDER
PRINT N'Alter table dbo.PIMS_INTEREST_HOLDER'
GO
ALTER TABLE [dbo].[PIMS_INTEREST_HOLDER]
	ADD [LEGACY_AO_REP_SOL_ID] bigint NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the Acquisition Owner solicitors and representatives to 
-- PIMS_INTEREST_HOLDER while capturing the new PK to realign the 
-- PIMS_ACQUISITION_PAYEE entries to the migrated interest holders.

-- Migrate the existing data to the PIMS_INTEREST_HOLDER table
INSERT INTO PIMS_INTEREST_HOLDER (ACQUISITION_FILE_ID, PERSON_ID, ORGANIZATION_ID, INTEREST_HOLDER_TYPE_CODE, COMMENT, IS_DISABLED, LEGACY_AO_REP_SOL_ID)
  OUTPUT inserted.INTEREST_HOLDER_ID
       , inserted.LEGACY_AO_REP_SOL_ID
  INTO   TMP_INTEREST_HOLDER_TXN (NEW_AO_REP_SOL_ID, LEGACY_AO_REP_SOL_ID)
SELECT ACQUISITION_FILE_ID
     , PERSON_ID
     , ORGANIZATION_ID
     , INTEREST_HOLDER_TYPE_CODE
     , COMMENT
     , IS_DISABLED
     , LEGACY_AO_REP_SOL_ID
FROM   TMP_INTEREST_HOLDER tmp
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_INTEREST_HOLDER
PRINT N'Alter table dbo.PIMS_INTEREST_HOLDER'
GO
ALTER TABLE [dbo].[PIMS_INTEREST_HOLDER]
	DROP COLUMN  [LEGACY_AO_REP_SOL_ID]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the existing data in the PIMS_ACQUISITION_PAYEE table.
UPDATE pay
SET    pay.INTEREST_HOLDER_ID         = txn.NEW_AO_REP_SOL_ID
     , pay.CONCURRENCY_CONTROL_NUMBER = pay.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_ACQUISITION_PAYEE  pay                                                        JOIN
       TMP_ACQUISITION_PAYEE   tpy ON tpy.ACQUISITION_PAYEE_ID = pay.ACQUISITION_PAYEE_ID JOIN
       TMP_INTEREST_HOLDER_TXN txn ON txn.LEGACY_AO_REP_SOL_ID = tpy.LEGACY_AO_REP_SOL_ID
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing TMP_INTEREST_HOLDER temporary table
DROP TABLE IF EXISTS [dbo].[TMP_INTEREST_HOLDER] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing TMP_ACQUISITION_PAYEE temporary table
DROP TABLE IF EXISTS [dbo].[TMP_ACQUISITION_PAYEE] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing TMP_INTEREST_HOLDER_TXN temporary table
DROP TABLE IF EXISTS [dbo].[TMP_INTEREST_HOLDER_TXN] 
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
