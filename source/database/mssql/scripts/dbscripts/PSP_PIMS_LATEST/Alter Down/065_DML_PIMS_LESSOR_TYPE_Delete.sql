/* -----------------------------------------------------------------------------
Delete data from the PIMS_LESSOR_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Nov-22  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'UNK'

SELECT LESSOR_TYPE_CODE
FROM   PIMS_LESSOR_TYPE
WHERE  LESSOR_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE 
  FROM   PIMS_LESSOR_TYPE 
  WHERE  LESSOR_TYPE_CODE = @CurrCd;
  END

COMMIT TRANSACTION;
