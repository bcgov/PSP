/* -----------------------------------------------------------------------------
Migrate the data from TMP_PIMS_ACQUISITION_PAYEE to PIMS_ACQUISITION_PAYEE
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Aug-03  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the temporary entity for PIMS_ACQUISITION_PAYEE
INSERT INTO [dbo].[PIMS_ACQUISITION_PAYEE] (COMPENSATION_REQUISITION_ID, ACQUISITION_OWNER_ID, INTEREST_HOLDER_ID, ACQUISITION_FILE_PERSON_ID, GST_NUMBER, IS_PAYMENT_IN_TRUST)
SELECT COMPENSATION_REQUISITION_ID
     , ACQUISITION_OWNER_ID
     , INTEREST_HOLDER_ID
     , ACQUISITION_FILE_PERSON_ID
     , GST_NUMBER
     , IS_PAYMENT_IN_TRUST
FROM   TMP_PIMS_ACQUISITION_PAYEE
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing TMP_PIMS_ACQUISITION_PAYEE temporary table
DROP TABLE IF EXISTS [dbo].[TMP_PIMS_ACQUISITION_PAYEE] 
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
