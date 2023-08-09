/* -----------------------------------------------------------------------------
Migrate the data to PIMS_COMPENSATION_REQUISITION.
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

-- Migrate the Payee data to PIMS_COMPENSATION_REQUISITION
UPDATE req
SET    req.ACQUISITION_OWNER_ID       = tmp.ACQUISITION_OWNER_ID
     , req.INTEREST_HOLDER_ID         = tmp.INTEREST_HOLDER_ID
     , req.ACQUISITION_FILE_PERSON_ID = tmp.ACQUISITION_FILE_PERSON_ID
     , req.GST_NUMBER                 = tmp.GST_NUMBER
     , req.IS_PAYMENT_IN_TRUST        = tmp.IS_PAYMENT_IN_TRUST
     , req.CONCURRENCY_CONTROL_NUMBER = req.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_COMPENSATION_REQUISITION req JOIN
       TMP_PIMS_ACQUISITION_PAYEE    tmp ON tmp.COMPENSATION_REQUISITION_ID = req.COMPENSATION_REQUISITION_ID
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
