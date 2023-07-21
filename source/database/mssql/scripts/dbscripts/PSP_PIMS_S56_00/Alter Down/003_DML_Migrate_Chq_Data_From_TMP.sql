/* -----------------------------------------------------------------------------
Migrate the data to the ACQ_PAYEE_CHEQUE table from the temporary table.
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
INSERT INTO PIMS_ACQ_PAYEE_CHEQUE(ACQUISITION_PAYEE_ID, GST_NUMBER, IS_PAYMENT_IN_TRUST)
SELECT pay.ACQUISITION_PAYEE_ID
     , tmp.GST_NUMBER
     , tmp.IS_PAYMENT_IN_TRUST
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
