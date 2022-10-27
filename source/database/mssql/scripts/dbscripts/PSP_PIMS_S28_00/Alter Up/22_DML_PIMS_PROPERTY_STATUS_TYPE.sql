/* -----------------------------------------------------------------------------
Insert data into the PIMS_PROPERTY_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

SELECT PROPERTY_STATUS_TYPE_CODE
FROM   PIMS_PROPERTY_STATUS_TYPE
WHERE  PROPERTY_STATUS_TYPE_CODE = N'UNKNOWN';

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_PROPERTY_STATUS_TYPE (PROPERTY_STATUS_TYPE_CODE, DESCRIPTION)
  VALUES
    (N'UNKNOWN', N'Unknown');
  END

COMMIT TRANSACTION;
