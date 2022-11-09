/* -----------------------------------------------------------------------------
Delete data from the PIMS_ACTIVITY_TEMPLATE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Sep-30  Initial version
Doug Filteau  2022-Oct-31  Added 'File Document'
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'FILEDOC'

SELECT ACTIVITY_TEMPLATE_TYPE_CODE
FROM   PIMS_ACTIVITY_TEMPLATE
WHERE  ACTIVITY_TEMPLATE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE 
  FROM   PIMS_ACTIVITY_TEMPLATE
  WHERE  ACTIVITY_TEMPLATE_TYPE_CODE = @CurrCd;
  END

COMMIT TRANSACTION;
