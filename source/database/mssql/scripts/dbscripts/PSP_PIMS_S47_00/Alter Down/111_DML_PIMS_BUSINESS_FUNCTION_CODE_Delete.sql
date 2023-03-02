/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_BUSINESS_FUNCTION_CODE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'MANAGE'

SELECT CODE
FROM   PIMS_BUSINESS_FUNCTION_CODE
WHERE  CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_BUSINESS_FUNCTION_CODE
  WHERE  CODE = @CurrCd;
  END

SET @CurrCd = N'ORIO ACQ'

SELECT CODE
FROM   PIMS_BUSINESS_FUNCTION_CODE
WHERE  CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_BUSINESS_FUNCTION_CODE
  WHERE  CODE = @CurrCd;
  END

SET @CurrCd = N'PROP AQU'

SELECT CODE
FROM   PIMS_BUSINESS_FUNCTION_CODE
WHERE  CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_BUSINESS_FUNCTION_CODE
  WHERE  CODE = @CurrCd;
  END

SET @CurrCd = N'PROP MAN'

SELECT CODE
FROM   PIMS_BUSINESS_FUNCTION_CODE
WHERE  CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_BUSINESS_FUNCTION_CODE
  WHERE  CODE = @CurrCd;
  END

COMMIT TRANSACTION;
