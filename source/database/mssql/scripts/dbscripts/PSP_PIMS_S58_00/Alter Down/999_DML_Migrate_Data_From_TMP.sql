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
    [NEW_AO_REP_SOL_ID]         BIGINT,
    [LEGACY_AO_REP_SOL_ID]      BIGINT,
    [INTEREST_HOLDER_TYPE_CODE] NVARCHAR(20)
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_ACQUISITION_OWNER_REP
PRINT N'Alter table dbo.PIMS_ACQUISITION_OWNER_REP'
GO
ALTER TABLE [dbo].[PIMS_ACQUISITION_OWNER_REP]
	ADD [LEGACY_AO_REP_SOL_ID] bigint NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_ACQUISITION_OWNER_SOLICITOR
PRINT N'Alter table dbo.PIMS_ACQUISITION_OWNER_SOLICITOR'
GO
ALTER TABLE [dbo].[PIMS_ACQUISITION_OWNER_SOLICITOR]
	ADD [LEGACY_AO_REP_SOL_ID] bigint NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the Interest Holders to the Acquisition Owner Ssolicitor
-- table while capturing the new and legacy PK to realign the 
-- PIMS_ACQUISITION_PAYEE entries to the migrated interest holders.

-- Migrate the existing data to the PIMS_ACQUISITION_OWNER_REP table
INSERT INTO PIMS_ACQUISITION_OWNER_REP (ACQUISITION_FILE_ID, PERSON_ID, COMMENT, IS_DISABLED, LEGACY_AO_REP_SOL_ID)
  OUTPUT inserted.OWNER_REPRESENTATIVE_ID
       , inserted.LEGACY_AO_REP_SOL_ID
       , N'AOREP'
  INTO   TMP_INTEREST_HOLDER_TXN (NEW_AO_REP_SOL_ID, LEGACY_AO_REP_SOL_ID, INTEREST_HOLDER_TYPE_CODE)
SELECT ACQUISITION_FILE_ID
     , PERSON_ID
     , COMMENT
     , IS_DISABLED
     , INTEREST_HOLDER_ID
FROM   TMP_INTEREST_HOLDER
WHERE  INTEREST_HOLDER_TYPE_CODE = N'AOREP'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the PIMS_ACQUISITION_OWNER_SOLICITOR table
INSERT INTO PIMS_ACQUISITION_OWNER_SOLICITOR (ACQUISITION_FILE_ID, PERSON_ID, ORGANIZATION_ID, IS_DISABLED, LEGACY_AO_REP_SOL_ID)
  OUTPUT inserted.OWNER_SOLICITOR_ID
       , inserted.LEGACY_AO_REP_SOL_ID
       , N'AOSLCTR'
  INTO   TMP_INTEREST_HOLDER_TXN (NEW_AO_REP_SOL_ID, LEGACY_AO_REP_SOL_ID, INTEREST_HOLDER_TYPE_CODE)
SELECT ACQUISITION_FILE_ID
     , PERSON_ID
     , ORGANIZATION_ID
     , IS_DISABLED
     , INTEREST_HOLDER_ID
FROM   TMP_INTEREST_HOLDER
WHERE  INTEREST_HOLDER_TYPE_CODE = N'AOSLCTR'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the existing data in the PIMS_ACQUISITION_PAYEE table
-- for the Owner Representative.
UPDATE pay
SET    pay.OWNER_REPRESENTATIVE_ID    = txn.NEW_AO_REP_SOL_ID
     , pay.INTEREST_HOLDER_ID         = NULL
     , pay.CONCURRENCY_CONTROL_NUMBER = pay.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_ACQUISITION_PAYEE  pay                                                              JOIN
       TMP_ACQUISITION_PAYEE   tpy ON  tpy.ACQUISITION_PAYEE_ID      = pay.ACQUISITION_PAYEE_ID 
                                   AND tpy.INTEREST_HOLDER_TYPE_CODE = N'AOREP'                 JOIN
       TMP_INTEREST_HOLDER_TXN txn ON  txn.LEGACY_AO_REP_SOL_ID      = tpy.INTEREST_HOLDER_ID
                                   AND txn.INTEREST_HOLDER_TYPE_CODE = N'AOREP'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the existing data in the PIMS_ACQUISITION_PAYEE table
-- for the Owner Solicitor.
UPDATE pay
SET    pay.OWNER_SOLICITOR_ID         = txn.NEW_AO_REP_SOL_ID
     , pay.INTEREST_HOLDER_ID         = NULL
     , pay.CONCURRENCY_CONTROL_NUMBER = pay.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_ACQUISITION_PAYEE  pay                                                              JOIN
       TMP_ACQUISITION_PAYEE   tpy ON  tpy.ACQUISITION_PAYEE_ID      = pay.ACQUISITION_PAYEE_ID 
                                   AND tpy.INTEREST_HOLDER_TYPE_CODE = N'AOSLCTR'               JOIN
       TMP_INTEREST_HOLDER_TXN txn ON  txn.LEGACY_AO_REP_SOL_ID      = tpy.INTEREST_HOLDER_ID
                                   AND txn.INTEREST_HOLDER_TYPE_CODE = N'AOSLCTR'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_ACQUISITION_OWNER_REP
PRINT N'Alter table dbo.PIMS_ACQUISITION_OWNER_REP'
GO
ALTER TABLE [dbo].[PIMS_ACQUISITION_OWNER_REP]
	DROP COLUMN  [LEGACY_AO_REP_SOL_ID]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_ACQUISITION_OWNER_SOLICITOR
PRINT N'Alter table dbo.PIMS_ACQUISITION_OWNER_SOLICITOR'
GO
ALTER TABLE [dbo].[PIMS_ACQUISITION_OWNER_SOLICITOR]
	DROP COLUMN  [LEGACY_AO_REP_SOL_ID]
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
