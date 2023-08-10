/* -----------------------------------------------------------------------------
Migrate the data from PIMS_FORM_8 to the temporary table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jul-27  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table for Form 8
DROP TABLE IF EXISTS [dbo].[TMP_FORM_8] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table for the Expropriation data
CREATE TABLE [dbo].[TMP_FORM_8] (
    [ACQUISITION_FILE_ID]     BIGINT,
    [ACQUISITION_OWNER_ID]    BIGINT,
    [INTEREST_HOLDER_ID]      BIGINT,
    [EXPROPRIATING_AUTHORITY] BIGINT,
    [PAYMENT_ITEM_TYPE_CODE]  NVARCHAR(20),
    [DESCRIPTION]             NVARCHAR(200),
    [IS_GST_REQUIRED]         BIT,
    [PRETAX_AMT]              MONEY,
    [TAX_AMT]                 MONEY,
    [TOTAL_AMT]               MONEY,
    [IS_DISABLED]             BIT)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the temporary entity for PIMS_FORM_8
INSERT INTO [dbo].[TMP_FORM_8] (ACQUISITION_FILE_ID, ACQUISITION_OWNER_ID, INTEREST_HOLDER_ID, EXPROPRIATING_AUTHORITY, PAYMENT_ITEM_TYPE_CODE, DESCRIPTION, IS_GST_REQUIRED, PRETAX_AMT, TAX_AMT, TOTAL_AMT, IS_DISABLED)
SELECT pmt.ACQUISITION_FILE_ID
     , pmt.ACQUISITION_OWNER_ID
     , pmt.INTEREST_HOLDER_ID
     , pmt.EXPROPRIATING_AUTHORITY
     , itm.PAYMENT_ITEM_TYPE_CODE
     , pmt.DESCRIPTION
     , itm.IS_GST_REQUIRED
     , itm.PRETAX_AMT
     , itm.TAX_AMT
     , itm.TOTAL_AMT
     , itm.IS_DISABLED
FROM   PIMS_EXPROPRIATION_PAYMENT pmt JOIN
       PIMS_EXPROP_PMT_PMT_ITEM   itm ON itm.EXPROPRIATION_PAYMENT_ID = pmt.EXPROPRIATION_PAYMENT_ID
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
