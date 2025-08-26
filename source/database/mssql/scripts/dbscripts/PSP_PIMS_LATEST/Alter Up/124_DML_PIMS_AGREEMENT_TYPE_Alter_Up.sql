/* -----------------------------------------------------------------------------
Alter the PIMS_AGREEMENT_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Aug-18  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/Enable the "H179FSPART" type.
PRINT N'Add/Enable the "H179FSPART" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'H179FSPART'

SELECT AGREEMENT_TYPE_CODE
FROM   PIMS_AGREEMENT_TYPE
WHERE  AGREEMENT_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_AGREEMENT_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  AGREEMENT_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_AGREEMENT_TYPE (AGREEMENT_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
  VALUES (N'H179FSPART', N'Agreement of Purchase and Sale H0179 (FS Part)', 6);  
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
