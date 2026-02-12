-- -------------------------------------------------------------------------------------------
-- Alter the PIMS_DSP_CHKLST_ITEM_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2026-Jan-21  PSP-11131  Edit the description of XFRPLNREGD.
-- -------------------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Edit the description of XFRPLNREGD.
PRINT N'Edit the description of XFRPLNREGD.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'XFRPLNREGD'

SELECT DSP_CHKLST_ITEM_TYPE_CODE
FROM   PIMS_DSP_CHKLST_ITEM_TYPE
WHERE  DSP_CHKLST_SECTION_TYPE_CODE = N'SALEINFO'
   AND DSP_CHKLST_ITEM_TYPE_CODE    = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_DSP_CHKLST_ITEM_TYPE
  SET    DESCRIPTION                = N'Transfer/plan registered with LTSA'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DSP_CHKLST_SECTION_TYPE_CODE = N'SALEINFO'
     AND DSP_CHKLST_ITEM_TYPE_CODE    = @CurrCd;
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

-- Disable the STRNTHCLM code value
UPDATE PIMS_DSP_CHKLST_ITEM_TYPE
SET    EXPIRY_DATE                = NULL
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  DSP_CHKLST_SECTION_TYPE_CODE = 'REFRCONS'
   AND DSP_CHKLST_ITEM_TYPE_CODE    = 'STRNTHCLM'
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
