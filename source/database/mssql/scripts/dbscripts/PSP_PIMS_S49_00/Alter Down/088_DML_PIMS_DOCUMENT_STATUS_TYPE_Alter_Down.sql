/* -----------------------------------------------------------------------------
Update data in the PIMS_DOCUMENT_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Enable "Sent" document status type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'SENT'

SELECT DOCUMENT_STATUS_TYPE_CODE
FROM   PIMS_DOCUMENT_STATUS_TYPE
WHERE  DOCUMENT_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_DOCUMENT_STATUS_TYPE
  SET    IS_DISABLED                = CONVERT([bit],(0))
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DOCUMENT_STATUS_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Enable "Registered" document status type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'RGSTRD'

SELECT DOCUMENT_STATUS_TYPE_CODE
FROM   PIMS_DOCUMENT_STATUS_TYPE
WHERE  DOCUMENT_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_DOCUMENT_STATUS_TYPE
  SET    IS_DISABLED                = CONVERT([bit],(0))
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DOCUMENT_STATUS_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Enable "Unregistered" document status type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'UNREGD'

SELECT DOCUMENT_STATUS_TYPE_CODE
FROM   PIMS_DOCUMENT_STATUS_TYPE
WHERE  DOCUMENT_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_DOCUMENT_STATUS_TYPE
  SET    IS_DISABLED                = CONVERT([bit],(0))
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DOCUMENT_STATUS_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Set display order for all document status types to NULL
UPDATE PIMS_DOCUMENT_STATUS_TYPE
SET    DISPLAY_ORDER              = NULL
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
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
