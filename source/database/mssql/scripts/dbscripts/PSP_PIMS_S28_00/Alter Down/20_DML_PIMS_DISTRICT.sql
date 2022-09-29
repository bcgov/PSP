/* -----------------------------------------------------------------------------
Delete data from the PIMS_DISTRICT table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Apr-12  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

SELECT DISTRICT_CODE
FROM   PIMS_DISTRICT
WHERE  DISTRICT_CODE = 12;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE
  FROM   PIMS_DISTRICT
  WHERE  DISTRICT_CODE = 12;
  END

COMMIT TRANSACTION;
