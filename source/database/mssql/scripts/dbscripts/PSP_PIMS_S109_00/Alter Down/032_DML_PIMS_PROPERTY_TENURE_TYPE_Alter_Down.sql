/* -----------------------------------------------------------------------------
Alter the PIMS_PROPERTY_TENURE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Aug-11  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/Enable the "FSMOTI" type.
PRINT N'Alter the "FSMOTI" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'FSMOTI'

SELECT PROPERTY_TENURE_TYPE_CODE
FROM   PIMS_PROPERTY_TENURE_TYPE
WHERE  PROPERTY_TENURE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_PROPERTY_TENURE_TYPE
  SET    DESCRIPTION                = N'Fee Simple - MoTI'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROPERTY_TENURE_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/Enable the "FSCROWN" type.
PRINT N'Alter the "FSCROWN" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'FSCROWN'

SELECT PROPERTY_TENURE_TYPE_CODE
FROM   PIMS_PROPERTY_TENURE_TYPE
WHERE  PROPERTY_TENURE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_PROPERTY_TENURE_TYPE
  SET    DESCRIPTION                = N'Fee Simple - Crown (Non-MoTI)'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROPERTY_TENURE_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/Enable the "NSRWMOTI" type.
PRINT N'Alter the "NSRWMOTI" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'NSRWMOTI'

SELECT PROPERTY_TENURE_TYPE_CODE
FROM   PIMS_PROPERTY_TENURE_TYPE
WHERE  PROPERTY_TENURE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_PROPERTY_TENURE_TYPE
  SET    DESCRIPTION                = N'Non-SRW Interests - MoTI'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROPERTY_TENURE_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/Enable the "SRWMOTI" type.
PRINT N'Alter the "SRWMOTI" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'SRWMOTI'

SELECT PROPERTY_TENURE_TYPE_CODE
FROM   PIMS_PROPERTY_TENURE_TYPE
WHERE  PROPERTY_TENURE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_PROPERTY_TENURE_TYPE
  SET    DESCRIPTION                = N'Statutory Right of Way (SRW) - MoTI'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROPERTY_TENURE_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_PROPERTY_TENURE_TYPE biz JOIN
       (SELECT PROPERTY_TENURE_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_PROPERTY_TENURE_TYPE) seq  ON seq.PROPERTY_TENURE_TYPE_CODE = biz.PROPERTY_TENURE_TYPE_CODE
WHERE  biz.PROPERTY_TENURE_TYPE_CODE <> (N'UNKNOWN')
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order for the UNKNOWN code value
-- --------------------------------------------------------------
UPDATE PIMS_PROPERTY_TENURE_TYPE
SET    DISPLAY_ORDER              = 99
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  PROPERTY_TENURE_TYPE_CODE = N'UNKNOWN';
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
