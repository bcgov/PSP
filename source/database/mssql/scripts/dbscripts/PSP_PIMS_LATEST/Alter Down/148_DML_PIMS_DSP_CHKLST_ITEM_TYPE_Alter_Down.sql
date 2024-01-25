/* -----------------------------------------------------------------------------
Alter the data in the PIMS_DSP_CHKLST_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jan-10  Initial version
Doug Filteau  2024-Jan-24  Added reordering of display orders
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert the "SGNDXFRPPH" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'SGNDXFRPPH'

SELECT DSP_CHKLST_ITEM_TYPE_CODE
FROM   PIMS_DSP_CHKLST_ITEM_TYPE
WHERE  DSP_CHKLST_ITEM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE 
  FROM  PIMS_DSP_CHKLST_ITEM_TYPE 
  WHERE DSP_CHKLST_ITEM_TYPE_CODE = @CurrCd;
  -- Reorder the display order of the items that follow 'SGNDXFRPPH'
  UPDATE PIMS_DSP_CHKLST_ITEM_TYPE
  SET    DISPLAY_ORDER              = DISPLAY_ORDER - 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DISPLAY_ORDER                > 23
     AND DSP_CHKLST_SECTION_TYPE_CODE = N'SALEINFO'
  END
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
