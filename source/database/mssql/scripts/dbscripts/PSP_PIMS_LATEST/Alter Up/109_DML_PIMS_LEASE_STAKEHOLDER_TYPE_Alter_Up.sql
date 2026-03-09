-- -------------------------------------------------------------------------------------------
-- Alter the PIMS_LEASE_STAKEHOLDER_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2021-Feb-03  PSP-11108  Added OTHPAY and altered UNK.
-- Doug Filteau  2026-Feb-27  PSP-11279  Amended to repair UNK and added OTHRCV.
-- Doug Filteau  2026-Mar-05  PSP-11279  Amended to disable UNK.
-- -------------------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable the "UNK" type.
PRINT N'Disable the "UNK" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'UNK'

SELECT LEASE_STAKEHOLDER_TYPE_CODE
FROM   PIMS_LEASE_STAKEHOLDER_TYPE
WHERE  LEASE_STAKEHOLDER_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_STAKEHOLDER_TYPE
  SET    DESCRIPTION                = N'Unknown'
       , IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_STAKEHOLDER_TYPE_CODE = @CurrCd;
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
IF (@Success = 1) 
  PRINT 'The database update succeeded'
ELSE 
  BEGIN
  IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
    PRINT 'The database update failed'
  END
GO
