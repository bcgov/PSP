/* -----------------------------------------------------------------------------
Migrate the data to PIMS_EXPROP_PMT_PMT_ITEM.
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

-- =============================================================================
-- Form 8 ======================================================================
-- =============================================================================

-- Drop the existing temporary table for the transitional Form 8 entries
DROP TABLE IF EXISTS [dbo].[TMP_FORM_8_TXN] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table for the transitional interest holders
CREATE TABLE [dbo].[TMP_FORM_8_TXN] (
    [NEW_FORM_8_ID]    BIGINT,
    [LEGACY_FORM_8_ID] BIGINT
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_EXPROPRIATION_PAYMENT
PRINT N'Alter table dbo.PIMS_EXPROPRIATION_PAYMENT'
GO
ALTER TABLE [dbo].[PIMS_EXPROPRIATION_PAYMENT]
	DROP COLUMN IF EXISTS [LEGACY_FORM_8_ID]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table PIMS_EXPROPRIATION_PAYMENT
PRINT N'Alter table dbo.PIMS_EXPROPRIATION_PAYMENT'
GO
ALTER TABLE [dbo].[PIMS_EXPROPRIATION_PAYMENT]
	ADD [LEGACY_FORM_8_ID] bigint NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the Form 8 data to PIMS_EXPROPRIATION_PAYMENT while capturing the new 
-- PK to realign the PIMS_EXPROP_PMT_PMT_ITEM entries to the migrated Form 8 rows.

-- Migrate the Form 8 data to PIMS_EXPROPRIATION_PAYMENT
INSERT INTO PIMS_EXPROPRIATION_PAYMENT (ACQUISITION_FILE_ID, ACQUISITION_OWNER_ID, INTEREST_HOLDER_ID, EXPROPRIATING_AUTHORITY, DESCRIPTION, IS_DISABLED, LEGACY_FORM_8_ID)
  OUTPUT inserted.EXPROPRIATION_PAYMENT_ID
       , inserted.LEGACY_FORM_8_ID
  INTO   TMP_FORM_8_TXN (NEW_FORM_8_ID, LEGACY_FORM_8_ID)
SELECT ACQUISITION_FILE_ID
     , ACQUISITION_OWNER_ID
     , INTEREST_HOLDER_ID
     , EXPROPRIATING_AUTHORITY
     , DESCRIPTION
     , IS_DISABLED
     , FORM_8_ID
FROM   TMP_FORM_8 tmp
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_EXPROPRIATION_PAYMENT
PRINT N'Alter table dbo.PIMS_EXPROPRIATION_PAYMENT'
GO
ALTER TABLE [dbo].[PIMS_EXPROPRIATION_PAYMENT]
	DROP COLUMN IF EXISTS [LEGACY_FORM_8_ID]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the Form 8 data to PIMS_EXPROP_PMT_PMT_ITEM.
INSERT INTO PIMS_EXPROP_PMT_PMT_ITEM (EXPROPRIATION_PAYMENT_ID, PAYMENT_ITEM_TYPE_CODE, IS_GST_REQUIRED, PRETAX_AMT, TAX_AMT, TOTAL_AMT)
SELECT txn.NEW_FORM_8_ID
     , tmp.PAYMENT_ITEM_TYPE_CODE
     , tmp.IS_GST_REQUIRED
     , tmp.PRETAX_AMT
     , tmp.TAX_AMT
     , tmp.TOTAL_AMT
FROM   TMP_FORM_8     tmp JOIN
       TMP_FORM_8_TXN txn ON txn.LEGACY_FORM_8_ID = tmp.FORM_8_ID
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing TMP_FORM_8 temporary table
DROP TABLE IF EXISTS [dbo].[TMP_FORM_8] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing TMP_FORM_8 transaction table
DROP TABLE IF EXISTS [dbo].[TMP_FORM_8_TXN] 
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
