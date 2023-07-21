/* -----------------------------------------------------------------------------
Alter the data in the PIMS_FORM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jun-30  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Delete the "FORM1" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'FORM1'

SELECT FORM_TYPE_CODE
FROM   PIMS_FORM_TYPE
WHERE  FORM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_FORM_TYPE
  WHERE  FORM_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Delete the "FORM5" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'FORM5'

SELECT FORM_TYPE_CODE
FROM   PIMS_FORM_TYPE
WHERE  FORM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_FORM_TYPE
  WHERE  FORM_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Delete the "FORM8" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'FORM8'

SELECT FORM_TYPE_CODE
FROM   PIMS_FORM_TYPE
WHERE  FORM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_FORM_TYPE
  WHERE  FORM_TYPE_CODE = @CurrCd;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Delete the "FORM9" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'FORM9'

SELECT FORM_TYPE_CODE
FROM   PIMS_FORM_TYPE
WHERE  FORM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_FORM_TYPE
  WHERE  FORM_TYPE_CODE = @CurrCd;
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
