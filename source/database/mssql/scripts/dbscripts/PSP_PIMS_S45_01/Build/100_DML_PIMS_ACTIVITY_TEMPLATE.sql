/* -----------------------------------------------------------------------------
Insert data into the PIMS_ACTIVITY_TEMPLATE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Sep-30  Initial version
Doug Filteau  2022-Oct-31  Added 'File Document'
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'GENERAL'

SELECT ACTIVITY_TEMPLATE_TYPE_CODE
FROM   PIMS_ACTIVITY_TEMPLATE
WHERE  ACTIVITY_TEMPLATE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_ACTIVITY_TEMPLATE (ACTIVITY_TEMPLATE_TYPE_CODE, CONCURRENCY_CONTROL_NUMBER)
  VALUES 
    (N'GENERAL', 1);
  END
  
SET @CurrCd = N'FILEDOC'

SELECT ACTIVITY_TEMPLATE_TYPE_CODE
FROM   PIMS_ACTIVITY_TEMPLATE
WHERE  ACTIVITY_TEMPLATE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_ACTIVITY_TEMPLATE (ACTIVITY_TEMPLATE_TYPE_CODE, CONCURRENCY_CONTROL_NUMBER)
  VALUES 
    (N'FILEDOC', 1);
  END

COMMIT TRANSACTION;
