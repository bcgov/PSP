/* -----------------------------------------------------------------------------
Alter the PIMS_ACQUISITION_FUNDING_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Aug-12  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/Enable the "MOTIDST" type.
PRINT N'Alter the "MOTIDST" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'MOTIDST'

SELECT ACQUISITION_FUNDING_TYPE_CODE
FROM   PIMS_ACQUISITION_FUNDING_TYPE
WHERE  ACQUISITION_FUNDING_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_ACQUISITION_FUNDING_TYPE
  SET    DESCRIPTION                = N'MoTI District'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  ACQUISITION_FUNDING_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/Enable the "MOTIREG" type.
PRINT N'Alter the "MOTIREG" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'MOTIREG'

SELECT ACQUISITION_FUNDING_TYPE_CODE
FROM   PIMS_ACQUISITION_FUNDING_TYPE
WHERE  ACQUISITION_FUNDING_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_ACQUISITION_FUNDING_TYPE
  SET    DESCRIPTION                = N'MoTI Region'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  ACQUISITION_FUNDING_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_ACQUISITION_FUNDING_TYPE biz JOIN
       (SELECT ACQUISITION_FUNDING_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_ACQUISITION_FUNDING_TYPE) seq  ON seq.ACQUISITION_FUNDING_TYPE_CODE = biz.ACQUISITION_FUNDING_TYPE_CODE
WHERE  biz.ACQUISITION_FUNDING_TYPE_CODE <> (N'OTHER')
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order for the OTHER code value
-- --------------------------------------------------------------
UPDATE PIMS_ACQUISITION_FUNDING_TYPE
SET    DISPLAY_ORDER              = 99
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  ACQUISITION_FUNDING_TYPE_CODE = N'OTHER';
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
