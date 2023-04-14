/* -----------------------------------------------------------------------------
Alter the data in the PIMS_FORM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Mar-22  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert the H0074 code.
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'H0074'

SELECT FORM_TYPE_CODE
FROM   PIMS_FORM_TYPE
WHERE  FORM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_FORM_TYPE (FORM_TYPE_CODE, DESCRIPTION)
  VALUES
    (N'H0074', N'License of Occupation for Construction Access (H0074)');
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert the H0443 code.
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'H0443'

SELECT FORM_TYPE_CODE
FROM   PIMS_FORM_TYPE
WHERE  FORM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_FORM_TYPE (FORM_TYPE_CODE, DESCRIPTION)
  VALUES
    (N'H0443', N'Conditions of Entry (H0443)');
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
