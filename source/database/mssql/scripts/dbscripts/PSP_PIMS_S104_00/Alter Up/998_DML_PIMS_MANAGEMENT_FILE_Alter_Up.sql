/* -----------------------------------------------------------------------------
Alter the PIMS_MANAGEMENT_FILE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-May-07  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter the "PUBTRANS" type
PRINT N'Alter the "PUBTRANS" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'PUBTRANS'

SELECT MANAGEMENT_FILE_PURPOSE_TYPE_CODE
FROM   PIMS_MANAGEMENT_FILE
WHERE  MANAGEMENT_FILE_PURPOSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_MANAGEMENT_FILE
  SET    MANAGEMENT_FILE_PURPOSE_TYPE_CODE = N'BCTRANS'
       , CONCURRENCY_CONTROL_NUMBER        = CONCURRENCY_CONTROL_NUMBER + 1
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
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
   IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
   PRINT 'The database update failed'
END
GO
