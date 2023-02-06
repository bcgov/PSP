/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_WORK_ACTIVITY_CODE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'PROP MAN'

SELECT CODE
FROM   PIMS_WORK_ACTIVITY_CODE
WHERE  CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_WORK_ACTIVITY_CODE
  WHERE  CODE = @CurrCd;
  END

SET @CurrCd = N'SURVEY'

SELECT CODE
FROM   PIMS_WORK_ACTIVITY_CODE
WHERE  CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_WORK_ACTIVITY_CODE
  WHERE  CODE = @CurrCd;
  END

SET @CurrCd = N'ELEC MAIN'

SELECT CODE
FROM   PIMS_WORK_ACTIVITY_CODE
WHERE  CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_WORK_ACTIVITY_CODE
  WHERE  CODE = @CurrCd;
  END

SET @CurrCd = N'ACQ PROP'

SELECT CODE
FROM   PIMS_WORK_ACTIVITY_CODE
WHERE  CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_WORK_ACTIVITY_CODE
  WHERE  CODE = @CurrCd;
  END

SET @CurrCd = N'PA GENERAL'

SELECT CODE
FROM   PIMS_WORK_ACTIVITY_CODE
WHERE  CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_WORK_ACTIVITY_CODE
  WHERE  CODE = @CurrCd;
  END

COMMIT TRANSACTION;
