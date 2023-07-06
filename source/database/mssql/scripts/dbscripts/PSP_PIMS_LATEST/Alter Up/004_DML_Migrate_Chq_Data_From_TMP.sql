/* -----------------------------------------------------------------------------
Migrate the data to the PIMS_ACQUISITION_PAYEE table from the temporary table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jun-08  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the PIMS_ACQUISITION_PAYEE table
UPDATE pay
SET    pay.GST_NUMBER                 = tmp.GST_NUMBER
     , pay.IS_PAYMENT_IN_TRUST        = tmp.IS_PAYMENT_IN_TRUST
     , pay.CONCURRENCY_CONTROL_NUMBER = pay.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_ACQUISITION_PAYEE pay JOIN
       TMP_ACQ_PAYEE_CHEQUE   tmp ON tmp.COMPENSATION_REQUISITION_ID = pay.COMPENSATION_REQUISITION_ID
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table for PERSONs
DROP TABLE IF EXISTS [dbo].[TMP_ACQ_PAYEE_CHEQUE] 
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
