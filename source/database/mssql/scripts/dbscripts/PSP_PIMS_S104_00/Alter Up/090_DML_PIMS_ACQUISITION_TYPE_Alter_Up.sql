/* -----------------------------------------------------------------------------
Alter the PIMS_ACQUISITION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-May-01  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert/Enable the "SECTN16" type
PRINT N'Insert/Enable the "SECTN16" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'SECTN16'

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
    VALUES (N'SECTN16',  N'Land Act - Section 16 Map Reserve');
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
