/* -----------------------------------------------------------------------------
Insert data into the PIMS_ACTIVITY_TEMPLATE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Oct-31 Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'FILEDOC'

SELECT ACTIVITY_TEMPLATE_TYPE_CODE
FROM   PIMS_ACTIVITY_TEMPLATE_TYPE
WHERE  ACTIVITY_TEMPLATE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_ACTIVITY_TEMPLATE_TYPE (ACTIVITY_TEMPLATE_TYPE_CODE, DESCRIPTION)
  VALUES 
    (N'FILEDOC', N'File Document');
  END
ELSE
  BEGIN
  UPDATE PIMS_ACTIVITY_TEMPLATE_TYPE
  SET    IS_DISABLED = CONVERT([bit],(0))
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  ACTIVITY_TEMPLATE_TYPE_CODE = @CurrCd;
  END

COMMIT TRANSACTION;
