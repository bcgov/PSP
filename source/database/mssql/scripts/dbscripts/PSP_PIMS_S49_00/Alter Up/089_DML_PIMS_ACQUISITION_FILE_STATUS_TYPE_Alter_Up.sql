/* -----------------------------------------------------------------------------
Insert data into the PIMS_ACQUISITION_FILE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Oct-31 Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable "Closed" status type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'CLOSED'

SELECT ACQUISITION_FILE_STATUS_TYPE_CODE
FROM   PIMS_ACQUISITION_FILE_STATUS_TYPE
WHERE  ACQUISITION_FILE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_ACQUISITION_FILE_STATUS_TYPE 
  SET    IS_DISABLED                = CONVERT([bit],(1))
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  ACQUISITION_FILE_STATUS_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert "Complete" status type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'COMPLT'

SELECT ACQUISITION_FILE_STATUS_TYPE_CODE
FROM   PIMS_ACQUISITION_FILE_STATUS_TYPE
WHERE  ACQUISITION_FILE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_ACQUISITION_FILE_STATUS_TYPE (ACQUISITION_FILE_STATUS_TYPE_CODE, DESCRIPTION)
  VALUES (N'COMPLT', N'Complete')
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
