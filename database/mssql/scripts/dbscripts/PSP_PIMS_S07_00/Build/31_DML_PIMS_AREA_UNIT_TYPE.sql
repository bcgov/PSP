/* -----------------------------------------------------------------------------
Delete all data from the PIMS_AREA_UNIT_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-09  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_AREA_UNIT_TYPE
GO

INSERT INTO PIMS_AREA_UNIT_TYPE (AREA_UNIT_TYPE_CODE, DESCRIPTION)
VALUES
  (N'HA', N'Hectare'),
  (N'M2', N'Meters sq');