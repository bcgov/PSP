/* -----------------------------------------------------------------------------
Alter the data in the PIMS_DSP_CHKLST_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jan-10  Initial version
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

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_DSP_CHKLST_ITEM_TYPE (DSP_CHKLST_SECTION_TYPE_CODE, DSP_CHKLST_ITEM_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER, HINT)
    VALUES
      (N'DIRCTSAL', N'SGNDXFRPPH', N'Signed Transfer Closed PPH Between Gov. Agencies', 24, 'Signed Transfer of Discontinued and Closed Provincial Public Highway Lands To The Minister Responsible');
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
