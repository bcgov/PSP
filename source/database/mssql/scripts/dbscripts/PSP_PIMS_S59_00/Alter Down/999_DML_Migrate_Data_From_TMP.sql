/* -----------------------------------------------------------------------------
Migrate the data to the PIMS_FORM_8 from the temporary tables.
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

-- Migrate the existing data to the PIMS_FORM_8 table
INSERT INTO PIMS_FORM_8 (ACQUISITION_FILE_ID, ACQUISITION_OWNER_ID, INTEREST_HOLDER_ID, EXPROPRIATING_AUTHORITY, PAYMENT_ITEM_TYPE_CODE, DESCRIPTION, IS_GST_REQUIRED, PRETAX_AMT, TAX_AMT, TOTAL_AMT, IS_DISABLED)
SELECT ACQUISITION_FILE_ID
     , ACQUISITION_OWNER_ID
     , INTEREST_HOLDER_ID
     , EXPROPRIATING_AUTHORITY
     , PAYMENT_ITEM_TYPE_CODE
     , DESCRIPTION
     , IS_GST_REQUIRED
     , PRETAX_AMT
     , TAX_AMT
     , TOTAL_AMT
     , IS_DISABLED
FROM   TMP_FORM_8
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing TMP_FORM_8 temporary table
DROP TABLE IF EXISTS [dbo].[TMP_FORM_8] 
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
