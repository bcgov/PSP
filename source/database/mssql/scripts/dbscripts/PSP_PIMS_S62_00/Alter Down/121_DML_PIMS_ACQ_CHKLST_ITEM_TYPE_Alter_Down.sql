/* -----------------------------------------------------------------------------
Alter the data in the PIMS_ACQ_CHKLST_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Aug-22  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "S3APPRCOPY" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'S3APPRCOPY'

SELECT ACQ_CHKLST_ITEM_TYPE_CODE
FROM   PIMS_ACQ_CHKLST_ITEM_TYPE
WHERE  ACQ_CHKLST_ITEM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_ACQ_CHKLST_ITEM_TYPE 
  SET    DESCRIPTION                = N'Copy of appraisial to owner(s)'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  ACQ_CHKLST_ITEM_TYPE_CODE = N'S3APPRCOPY';
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "S6APPRCOPY" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'S6APPRCOPY'

SELECT ACQ_CHKLST_ITEM_TYPE_CODE
FROM   PIMS_ACQ_CHKLST_ITEM_TYPE
WHERE  ACQ_CHKLST_ITEM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_ACQ_CHKLST_ITEM_TYPE 
  SET    DESCRIPTION                = N'Copy of appraisial to owner(s)'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  ACQ_CHKLST_ITEM_TYPE_CODE = N'S6APPRCOPY';
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
