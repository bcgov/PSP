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

-- Disable ADCOMPLT code.
PRINT N'Disable ADCOMPLT code.'
GO
SELECT DSP_CHKLST_ITEM_TYPE_CODE
FROM   PIMS_DSP_CHKLST_ITEM_TYPE
WHERE  DSP_CHKLST_SECTION_TYPE_CODE = N'DIRCTSAL'
   AND DSP_CHKLST_ITEM_TYPE_CODE    = N'ADCOMPLT';

IF @@ROWCOUNT = 1
  UPDATE PIMS_DSP_CHKLST_ITEM_TYPE
  SET    EXPIRY_DATE                = CAST('2025-01-01' AS DATE)
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DSP_CHKLST_SECTION_TYPE_CODE = N'DIRCTSAL' 
     AND DSP_CHKLST_ITEM_TYPE_CODE    = N'ADCOMPLT';
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable ADJLOREF code.
PRINT N'Disable ADJLOREF code.'
GO
SELECT DSP_CHKLST_ITEM_TYPE_CODE
FROM   PIMS_DSP_CHKLST_ITEM_TYPE
WHERE  DSP_CHKLST_SECTION_TYPE_CODE = N'DIRCTSAL'
   AND DSP_CHKLST_ITEM_TYPE_CODE    = N'ADJLOREF';

IF @@ROWCOUNT = 1
  UPDATE PIMS_DSP_CHKLST_ITEM_TYPE
  SET    EXPIRY_DATE                = CAST('2025-01-01' AS DATE)
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DSP_CHKLST_SECTION_TYPE_CODE = N'DIRCTSAL' 
     AND DSP_CHKLST_ITEM_TYPE_CODE    = N'ADJLOREF';
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable UTILCOAPP code.
PRINT N'Disable UTILCOAPP code.'
GO
SELECT DSP_CHKLST_ITEM_TYPE_CODE
FROM   PIMS_DSP_CHKLST_ITEM_TYPE
WHERE  DSP_CHKLST_SECTION_TYPE_CODE = N'DIRCTSAL'
   AND DSP_CHKLST_ITEM_TYPE_CODE    = N'UTILCOAPP';

IF @@ROWCOUNT = 1
  UPDATE PIMS_DSP_CHKLST_ITEM_TYPE
  SET    EXPIRY_DATE                = CAST('2025-01-01' AS DATE)
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DSP_CHKLST_SECTION_TYPE_CODE = N'DIRCTSAL' 
     AND DSP_CHKLST_ITEM_TYPE_CODE    = N'UTILCOAPP';
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Enable the "LITIGATN" type.
PRINT N'Enable the "LITIGATN" type.'
GO
SELECT DSP_CHKLST_ITEM_TYPE_CODE
FROM   PIMS_DSP_CHKLST_ITEM_TYPE
WHERE  DSP_CHKLST_SECTION_TYPE_CODE = N'REFRCONS'
   AND DSP_CHKLST_ITEM_TYPE_CODE    = N'LITIGATN';

IF @@ROWCOUNT = 1
  UPDATE PIMS_DSP_CHKLST_ITEM_TYPE
  SET    EXPIRY_DATE                = NULL
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DSP_CHKLST_SECTION_TYPE_CODE = N'REFRCONS'
     AND DSP_CHKLST_ITEM_TYPE_CODE    = N'LITIGATN';
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
