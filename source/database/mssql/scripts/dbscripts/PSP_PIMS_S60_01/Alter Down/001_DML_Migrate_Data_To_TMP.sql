/* -----------------------------------------------------------------------------
Migrate the data from the PIMS_EXPROP_PMT_PMT_ITEM to the temporary tables.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jul-28  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table for the Expropriation data
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
    [DESCRIPTION]             NVARCHAR(2000),
    [IS_DISABLED]             BIT,
    [PAYMENT_ITEM_TYPE_CODE]  NVARCHAR(20),
    [IS_GST_REQUIRED]         BIT,
    [PRETAX_AMT]              MONEY,
    [TAX_AMT]                 MONEY,
    [TOTAL_AMT]               MONEY)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the temporary entity
INSERT INTO [dbo].[TMP_FORM_8] (ACQUISITION_FILE_ID, ACQUISITION_OWNER_ID, INTEREST_HOLDER_ID, EXPROPRIATING_AUTHORITY, DESCRIPTION, IS_DISABLED, PAYMENT_ITEM_TYPE_CODE, IS_GST_REQUIRED, PRETAX_AMT, TAX_AMT, TOTAL_AMT)
SELECT pmt.ACQUISITION_FILE_ID
     , pmt.ACQUISITION_OWNER_ID
     , pmt.INTEREST_HOLDER_ID
     , pmt.EXPROPRIATING_AUTHORITY
     , pmt.DESCRIPTION
     , pmt.IS_DISABLED
     , pit.PAYMENT_ITEM_TYPE_CODE
     , pit.IS_GST_REQUIRED
     , pit.PRETAX_AMT
     , pit.TAX_AMT
     , pit.TOTAL_AMT
FROM   PIMS_EXPROPRIATION_PAYMENT pmt JOIN
       PIMS_EXPROP_PMT_PMT_ITEM   pit ON pit.EXPROPRIATION_PAYMENT_ID = pmt.EXPROPRIATION_PAYMENT_ID
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
