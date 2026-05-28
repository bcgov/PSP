/* -----------------------------------------------------------------------------
Alter the PIMS_LEASE_CHKLST_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Dec-09  Added 'LGLSRVOBTND' checklist items.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/enable UTILCOAPP code.
PRINT N'Add/enable UTILCOAPP code.'
GO
SELECT LEASE_CHKLST_ITEM_TYPE_CODE
FROM   PIMS_LEASE_CHKLST_ITEM_TYPE
WHERE  LEASE_CHKLST_SECTION_TYPE_CODE = N'AGREEPREP'
   AND LEASE_CHKLST_ITEM_TYPE_CODE    = N'LGLSRVOBTND';

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_CHKLST_SECTION_TYPE_CODE = N'AGREEPREP'
     AND LEASE_CHKLST_ITEM_TYPE_CODE    = N'LGLSRVOBTND';
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
