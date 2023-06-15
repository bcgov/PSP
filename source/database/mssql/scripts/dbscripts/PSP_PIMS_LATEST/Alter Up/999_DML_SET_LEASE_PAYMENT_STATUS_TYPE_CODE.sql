/* -----------------------------------------------------------------------------
Set ALL users to Ministry Staff in the PIMS_USER table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jun-15  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the null LEASE_PAYMENT_STATUS_TYPE_CODE
UPDATE pmt
SET    pmt.LEASE_PAYMENT_STATUS_TYPE_CODE = CASE
                                              WHEN pmt.PAYMENT_AMOUNT_TOTAL = trm.[PAYMENT AMOUNT] + ISNULL(trm.GST_AMOUNT, 0) THEN 'PAID'
                                              WHEN pmt.PAYMENT_AMOUNT_TOTAL < trm.[PAYMENT AMOUNT] + ISNULL(trm.GST_AMOUNT, 0) THEN 'PARTIAL'
                                              WHEN pmt.PAYMENT_AMOUNT_TOTAL > trm.[PAYMENT AMOUNT] + ISNULL(trm.GST_AMOUNT, 0) THEN 'OVERPAID'
                                              ELSE                                                                                  'UNPAID'
                                              END
     , pmt.CONCURRENCY_CONTROL_NUMBER     = pmt.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_LEASE_PAYMENT pmt JOIN
       PIMS_LEASE_TERM    trm ON trm.LEASE_TERM_ID = pmt.LEASE_TERM_ID
WHERE  pmt.LEASE_PAYMENT_STATUS_TYPE_CODE IS NULL
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
