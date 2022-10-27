/* -----------------------------------------------------------------------------
Delete data from the PIMS_REGION table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Apr-12  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

SELECT REGION_CODE
FROM   PIMS_REGION
WHERE  REGION_CODE = 4;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_REGION
  WHERE  REGION_CODE = 4;
  END

COMMIT TRANSACTION;
