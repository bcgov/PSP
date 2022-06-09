/* -----------------------------------------------------------------------------
Insert data into the PIMS_REGION table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Apr-12  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

SELECT REGION_CODE
FROM   PIMS_REGION
WHERE  REGION_CODE = 4;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_REGION (REGION_CODE, REGION_NAME) 
  VALUES
    (4, N'Cannot determine');
  END

COMMIT TRANSACTION;
