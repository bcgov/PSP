-- -------------------------------------------------------------------------------------------
-- Alter the PIMS_FORM_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2025-Dec-17  N/A        Added the FORM4, FORM6, and FORM7 type codes.
-- Doug Filteau  2026-Jan-08  PSP-11105  Added the H179B type code.
-- Doug Filteau  2026-Jan-08  PSP-11106  Added the H179D type code.
-- Doug Filteau  2026-Jan-08  PSP-11107  Added the H179RC type code.
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

SELECT FORM_TYPE_CODE
FROM   PIMS_FORM_TYPE
WHERE  FORM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_FORM_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  FORM_TYPE_CODE = @CurrCd;
ELSE
	INSERT INTO PIMS_FORM_TYPE (FORM_TYPE_CODE, DESCRIPTION)
	VALUES (N'H179B', N'Release of Claims Agreement (H-0179(B))');
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/enable H179D code.
PRINT N'Add/enable H179D code.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'H179D'

SELECT FORM_TYPE_CODE
FROM   PIMS_FORM_TYPE
WHERE  FORM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_FORM_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  FORM_TYPE_CODE = @CurrCd;
ELSE
	INSERT INTO PIMS_FORM_TYPE (FORM_TYPE_CODE, DESCRIPTION)
	VALUES (N'H179D', N'Statutory Right of Way Agreement (H-0179(D))');
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/enable H179RC code.
PRINT N'Add/enable H179RC code.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'H179RC'

SELECT FORM_TYPE_CODE
FROM   PIMS_FORM_TYPE
WHERE  FORM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_FORM_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  FORM_TYPE_CODE = @CurrCd;
ELSE
	INSERT INTO PIMS_FORM_TYPE (FORM_TYPE_CODE, DESCRIPTION)
	VALUES (N'H179RC', N'Agreement of Purchase and Sale (Closed Road) (H-0179(RC))');
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = biz.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_FORM_TYPE biz JOIN
       (SELECT FORM_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_FORM_TYPE) seq ON seq.FORM_TYPE_CODE = biz.FORM_TYPE_CODE
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
