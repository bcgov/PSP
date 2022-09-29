/* -----------------------------------------------------------------------------
Delete data from the PIMS_AREA_UNIT_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Mar-08  Initial version
----------------------------------------------------------------------------- */

DELETE 
FROM   PIMS_AREA_UNIT_TYPE
WHERE  AREA_UNIT_TYPE_CODE IN (N'ACRE', N'FEET2');