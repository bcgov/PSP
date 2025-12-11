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
  INSERT INTO PIMS_AGREEMENT_TYPE (AGREEMENT_TYPE_CODE, DESCRIPTION)
  VALUES (N'H179FSPART', N'Agreement of Purchase and Sale H0179 (FS Part)');  
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/Enable the "H179PTO" type.
PRINT N'Add/Enable the "H179PTO" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'H179PTO'

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
  VALUES (N'H179PTO', N'Optional Lease to Accompany H-179 (P) and H-179 (T)');  
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
