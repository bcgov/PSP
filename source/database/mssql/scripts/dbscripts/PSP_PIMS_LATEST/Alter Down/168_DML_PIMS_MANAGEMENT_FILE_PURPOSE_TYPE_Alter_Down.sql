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
PRINT N'Disable the "LITIGATN" type.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'LITIGATN'

SELECT MANAGEMENT_FILE_PURPOSE_TYPE_CODE
FROM   PIMS_MANAGEMENT_FILE_PURPOSE_TYPE
WHERE  MANAGEMENT_FILE_PURPOSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_MANAGEMENT_FILE_PURPOSE_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  MANAGEMENT_FILE_PURPOSE_TYPE_CODE = @CurrCd;
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
