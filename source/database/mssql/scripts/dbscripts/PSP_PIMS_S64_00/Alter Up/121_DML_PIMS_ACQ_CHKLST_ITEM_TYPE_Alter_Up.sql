/* -----------------------------------------------------------------------------
Alter the data in the PIMS_ACQ_CHKLST_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Sep-21  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter the display order of the codes below the newly inserted value.
DECLARE @cnt INT = 46

WHILE @cnt > 36
  BEGIN
  UPDATE PIMS_ACQ_CHKLST_ITEM_TYPE
  SET    DISPLAY_ORDER              = DISPLAY_ORDER + 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DISPLAY_ORDER = @cnt;
  SET @cnt = @cnt - 1;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert the "EXPROPAPPEAR" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'EXPROPAPPEAR'

SELECT ACQ_CHKLST_ITEM_TYPE_CODE
FROM   PIMS_ACQ_CHKLST_ITEM_TYPE
WHERE  ACQ_CHKLST_ITEM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_ACQ_CHKLST_ITEM_TYPE (ACQ_CHKLST_SECTION_TYPE_CODE, ACQ_CHKLST_ITEM_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER, HINT)
  VALUES 
    (N'SCTN6XPRP', N'EXPROPAPPEAR', N'Copy of approved EAR', 37, N'Executive approval to proceed with expropriation');
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
