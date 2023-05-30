/* -----------------------------------------------------------------------------
Add column IS_PAYMENT_IN_TRUST to PIMS_ACQ_PAYEE_CHEQUE.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-May-29  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add column AQPCHQ_IS_PAYMENT_IN_TRUST_DEF
PRINT N'Add column AQPCHQ_IS_PAYMENT_IN_TRUST_DEF'
GO
ALTER TABLE [dbo].[PIMS_ACQ_PAYEE_CHEQUE] 
  ADD [IS_PAYMENT_IN_TRUST] BIT CONSTRAINT [AQPCHQ_IS_PAYMENT_IN_TRUST_DEF] DEFAULT CONVERT([bit],(0))
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
