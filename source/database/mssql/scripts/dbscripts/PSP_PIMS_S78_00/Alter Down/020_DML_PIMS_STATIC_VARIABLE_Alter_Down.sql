/* -----------------------------------------------------------------------------
Alter the data in the PIMS_STATIC_VARIABLE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Mar-27  Initial version
Doug Filteau  2024-Apr-16  Updated FY Start and End.
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
SET @CurrVer = N'77.00'

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

-- Update the "FYSTART" variable/
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'FYSTART'

SELECT STATIC_VARIABLE_NAME
FROM   PIMS_STATIC_VARIABLE
WHERE  STATIC_VARIABLE_NAME = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_STATIC_VARIABLE
  SET    STATIC_VARIABLE_VALUE      = N'01/04/2023'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  STATIC_VARIABLE_NAME = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "FYEND" variable/
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'FYEND'

SELECT STATIC_VARIABLE_NAME
FROM   PIMS_STATIC_VARIABLE
WHERE  STATIC_VARIABLE_NAME = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_STATIC_VARIABLE
  SET    STATIC_VARIABLE_VALUE      = N'31/03/2024'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  STATIC_VARIABLE_NAME = @CurrCd;
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

