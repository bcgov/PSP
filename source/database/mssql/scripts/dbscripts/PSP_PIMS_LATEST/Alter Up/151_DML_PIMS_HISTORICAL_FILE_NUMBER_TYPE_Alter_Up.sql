/* -----------------------------------------------------------------------------
Alter the data in the PIMS_HISTORICAL_FILE_NUMBER_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-May-24  Initial version.  Rename descriptions for LISNO and
                           PSNO.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "LISNO" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'LISNO'

SELECT HISTORICAL_FILE_NUMBER_TYPE_CODE
FROM   PIMS_HISTORICAL_FILE_NUMBER_TYPE
WHERE  HISTORICAL_FILE_NUMBER_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_HISTORICAL_FILE_NUMBER_TYPE
  SET    DESCRIPTION                = N'LIS'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  HISTORICAL_FILE_NUMBER_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "PSNO" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'PSNO'

SELECT HISTORICAL_FILE_NUMBER_TYPE_CODE
FROM   PIMS_HISTORICAL_FILE_NUMBER_TYPE
WHERE  HISTORICAL_FILE_NUMBER_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_HISTORICAL_FILE_NUMBER_TYPE
  SET    DESCRIPTION                = N'PS'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  HISTORICAL_FILE_NUMBER_TYPE_CODE = @CurrCd;
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

