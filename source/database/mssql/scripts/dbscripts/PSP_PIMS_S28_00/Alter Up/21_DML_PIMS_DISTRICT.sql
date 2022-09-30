/* -----------------------------------------------------------------------------
Insert data into the PIMS_DISTRICT table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Apr-12  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

SELECT DISTRICT_CODE
FROM   PIMS_DISTRICT
WHERE  DISTRICT_CODE = 12;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_DISTRICT (DISTRICT_CODE, REGION_CODE, DISTRICT_NAME) 
  VALUES
    (12, 4, N'Cannot determine');
  END

COMMIT TRANSACTION;
