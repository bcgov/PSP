/* -----------------------------------------------------------------------------
Alter the PIMS_AGREEMENT_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Sep-03  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/Enable the "H179FS" type.
PRINT N'Add/Enable the "H179FS" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'H179FS'

SELECT AGREEMENT_TYPE_CODE
FROM   PIMS_AGREEMENT_TYPE
WHERE  AGREEMENT_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_AGREEMENT_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  AGREEMENT_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_AGREEMENT_TYPE (AGREEMENT_TYPE_CODE, DESCRIPTION)
  VALUES (N'H179FS', N'Fee Simple Agreement (H179FS)');  
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable the "TOTAL" type.
PRINT N'Disable the "TOTAL" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'TOTAL'

SELECT AGREEMENT_TYPE_CODE
FROM   PIMS_AGREEMENT_TYPE
WHERE  AGREEMENT_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_AGREEMENT_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  AGREEMENT_TYPE_CODE = @CurrCd;
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

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_AGREEMENT_TYPE biz JOIN
       (SELECT AGREEMENT_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_AGREEMENT_TYPE) seq  ON seq.AGREEMENT_TYPE_CODE = biz.AGREEMENT_TYPE_CODE
GO
