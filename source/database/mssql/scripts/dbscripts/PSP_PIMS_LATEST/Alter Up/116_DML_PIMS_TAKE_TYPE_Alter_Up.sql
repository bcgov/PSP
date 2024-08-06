/* -----------------------------------------------------------------------------
Alter the data in the PIMS_TAKE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Mar-27  Initial version.
Doug Filteau  2024-Apr-18  Add 'IMPORTED' take type.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert the "IMPORTED" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'IMPORTED'

SELECT TAKE_TYPE_CODE
FROM   PIMS_TAKE_TYPE
WHERE  TAKE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  INSERT INTO PIMS_TAKE_TYPE (TAKE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
    VALUES
      (N'IMPORTED', N'Imported', 1);
ELSE  
  UPDATE PIMS_TAKE_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  TAKE_TYPE_CODE = @CurrCd;
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

