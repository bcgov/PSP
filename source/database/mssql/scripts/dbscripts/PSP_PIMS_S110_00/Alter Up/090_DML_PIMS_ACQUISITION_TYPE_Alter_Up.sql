/* -----------------------------------------------------------------------------
Alter the PIMS_ACQUISITION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Sep-04  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/Enable the "CRWNTNR" type.
PRINT N'Add/Enable the "CRWNTNR" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'CRWNTNR'

SELECT ACQUISITION_TYPE_CODE
FROM   PIMS_ACQUISITION_TYPE
WHERE  ACQUISITION_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_ACQUISITION_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  ACQUISITION_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_ACQUISITION_TYPE (ACQUISITION_TYPE_CODE, DESCRIPTION)
  VALUES (N'CRWNTNR', N'Crown Tenure');  
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
FROM   PIMS_ACQUISITION_TYPE biz JOIN
       (SELECT ACQUISITION_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_ACQUISITION_TYPE) seq  ON seq.ACQUISITION_TYPE_CODE = biz.ACQUISITION_TYPE_CODE
GO
