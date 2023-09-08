/* -----------------------------------------------------------------------------
Update the database version in the PIMS_STATIC_VARIABLE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Nov-24  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the database version number.
DECLARE @CurrVer NVARCHAR(100)
SET @CurrVer = N'60.00'

UPDATE PIMS_STATIC_VARIABLE
WITH   (UPDLOCK, SERIALIZABLE) 
SET    STATIC_VARIABLE_VALUE      = @CurrVer
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  STATIC_VARIABLE_NAME       = N'DBVERSION';

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_STATIC_VARIABLE (STATIC_VARIABLE_NAME, STATIC_VARIABLE_VALUE)
    VALUES (N'DBVERSION', @CurrVer);
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