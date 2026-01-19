-- -------------------------------------------------------------------------------------------
-- Alter the PIMS_FORM_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2025-Dec-17  N/A        Added the FORM4, FORM6, and FORM7 type codes.
-- Doug Filteau  2026-Jan-08  PSP-11105  Added the H0179B type code.
-- Doug Filteau  2026-Jan-08  PSP-11106  Added the H0179D type code.
-- Doug Filteau  2026-Jan-08  PSP-11107  Added the H0179RC type code.
-- -------------------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable H0179B code.
PRINT N'Disable H0179B code.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'H0179B'

SELECT FORM_TYPE_CODE
FROM   PIMS_FORM_TYPE
WHERE  FORM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_FORM_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  FORM_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable H0179D code.
PRINT N'Disable H0179D code.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'H0179D'

SELECT FORM_TYPE_CODE
FROM   PIMS_FORM_TYPE
WHERE  FORM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_FORM_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  FORM_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable H0179RC code.
PRINT N'Disable H0179RC code.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'H0179RC'

SELECT FORM_TYPE_CODE
FROM   PIMS_FORM_TYPE
WHERE  FORM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_FORM_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  FORM_TYPE_CODE = @CurrCd;
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
