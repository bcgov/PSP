/* -----------------------------------------------------------------------------
Drop column IS_PAYMENT_IN_TRUST from PIMS_ACQ_PAYEE_CHEQUE.
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

-- Drop constraint AQPCHQ_IS_PAYMENT_IN_TRUST_DEF
PRINT N'Drop constraint AQPCHQ_IS_PAYMENT_IN_TRUST_DEF'
GO
ALTER TABLE [dbo].[PIMS_ACQ_PAYEE_CHEQUE] 
  DROP CONSTRAINT IF EXISTS [AQPCHQ_IS_PAYMENT_IN_TRUST_DEF]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop column IS_PAYMENT_IN_TRUST
PRINT N'Drop column IS_PAYMENT_IN_TRUST'
GO
ALTER TABLE [dbo].[PIMS_ACQ_PAYEE_CHEQUE] 
  DROP COLUMN IF EXISTS [IS_PAYMENT_IN_TRUST]
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
