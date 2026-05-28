/* -----------------------------------------------------------------------------
Alter the PIMS_MANAGEMENT_FILE_PURPOSE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Nov-19  Added EVCHARGER.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/Enable the "LITIGATN" type.
PRINT N'Add/Enable the "LITIGATN" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'LITIGATN'

SELECT MANAGEMENT_FILE_PURPOSE_TYPE_CODE
FROM   PIMS_MANAGEMENT_FILE_PURPOSE_TYPE
WHERE  MANAGEMENT_FILE_PURPOSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_MANAGEMENT_FILE_PURPOSE_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  MANAGEMENT_FILE_PURPOSE_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_MANAGEMENT_FILE_PURPOSE_TYPE (MANAGEMENT_FILE_PURPOSE_TYPE_CODE, DESCRIPTION)
  VALUES (N'LITIGATN', N'Litigation');  
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_MANAGEMENT_FILE_PURPOSE_TYPE biz JOIN
       (SELECT MANAGEMENT_FILE_PURPOSE_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_MANAGEMENT_FILE_PURPOSE_TYPE) seq  ON seq.MANAGEMENT_FILE_PURPOSE_TYPE_CODE = biz.MANAGEMENT_FILE_PURPOSE_TYPE_CODE
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
