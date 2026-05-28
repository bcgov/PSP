/* -----------------------------------------------------------------------------
Alter the PIMS_DSP_CHKLST_SECTION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Dec-08 Renamed DIRCTSAL desciption.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Rename the "DIRCTSAL" description.
PRINT N'Rename the "DIRCTSAL" description.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'DIRCTSAL'

SELECT DSP_CHKLST_SECTION_TYPE_CODE
FROM   PIMS_DSP_CHKLST_SECTION_TYPE
WHERE  DSP_CHKLST_SECTION_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_DSP_CHKLST_SECTION_TYPE
  SET    DESCRIPTION                = 'Direct Sale or Road Closure'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DSP_CHKLST_SECTION_TYPE_CODE = @CurrCd;
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
