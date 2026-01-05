/* -----------------------------------------------------------------------------
Alter the PIMS_DSP_CHKLST_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Dec-09  Removed 'UTILCOAPP', 'ADJLOREF', and 'ADCOMPLT' 
                           checklist items.
Doug Filteau  2025-Dec-09  Enabled 'STRNTHCLM' checklist item.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- NOTE ------------------------------------------------------------------------
-- Due to missing/conflicting requirements, the code valeus introduced in the 
-- 115 Alter Up scripts will not be deleted from the 114 version's table.
-- -----------------------------------------------------------------------------

---- Delete UTILCOAPP code.
--PRINT N'Delete UTILCOAPP code.'
--GO
--DELETE
--FROM   PIMS_DSP_CHKLST_ITEM_TYPE
--WHERE  DSP_CHKLST_SECTION_TYPE_CODE = N'DIRCTSAL'
--   AND DSP_CHKLST_ITEM_TYPE_CODE   IN (N'UTILCOAPP', N'ADJLOREF', N'ADCOMPLT');
--GO
--IF @@ERROR <> 0 SET NOEXEC ON
--GO

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
