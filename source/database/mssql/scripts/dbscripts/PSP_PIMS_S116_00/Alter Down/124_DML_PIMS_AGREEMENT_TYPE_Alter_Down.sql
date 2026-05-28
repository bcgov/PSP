-- -------------------------------------------------------------------------------------------
-- Alter the PIMS_AGREEMENT_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2026-Jan-19  PSP-11164  Added the H179B and H179D type code
-- -------------------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/enable H179B code.
PRINT N'Add/enable H179B code.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'H179B'

SELECT AGREEMENT_TYPE_CODE
FROM   PIMS_AGREEMENT_TYPE
WHERE  AGREEMENT_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_AGREEMENT_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  AGREEMENT_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/enable H179D code.
PRINT N'Add/enable H179D code.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'H179D'

SELECT AGREEMENT_TYPE_CODE
FROM   PIMS_AGREEMENT_TYPE
WHERE  AGREEMENT_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_AGREEMENT_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  AGREEMENT_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/enable H179RC code.
PRINT N'Add/enable H179RC code.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'H179RC'

SELECT AGREEMENT_TYPE_CODE
FROM   PIMS_AGREEMENT_TYPE
WHERE  AGREEMENT_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_AGREEMENT_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  AGREEMENT_TYPE_CODE = @CurrCd;
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
