/* -----------------------------------------------------------------------------
Insert data into the PIMS_AREA_UNIT_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Mar-08  Initial version
----------------------------------------------------------------------------- */

INSERT INTO PIMS_AREA_UNIT_TYPE (AREA_UNIT_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACRE', N'Acres'),
  (N'FEET2', N'Feet sq');