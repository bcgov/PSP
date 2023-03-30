/* -----------------------------------------------------------------------------
Convert data into the PIMS_ACQUISITION_FILE_STATUS_TYPE table from "COMPLT" to 
"CLOSED" status.
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

-- Convert "COMPLT" to "CLOSED"
UPDATE PIMS_ACQUISITION_FILE
SET    ACQUISITION_FILE_STATUS_TYPE_CODE = N'CLOSED'
     , CONCURRENCY_CONTROL_NUMBER        = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  ACQUISITION_FILE_STATUS_TYPE_CODE = N'COMPLT'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Delete "Complete" status type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'COMPLT'

SELECT ACQUISITION_FILE_STATUS_TYPE_CODE
FROM   PIMS_ACQUISITION_FILE_STATUS_TYPE
WHERE  ACQUISITION_FILE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE FROM PIMS_ACQUISITION_FILE_STATUS_TYPE
  WHERE  ACQUISITION_FILE_STATUS_TYPE_CODE = N'COMPLT'
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
