/* -----------------------------------------------------------------------------
Delete all data from the PIMS_VOLUME_UNIT_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_VOLUME_UNIT_TYPE
GO

INSERT INTO PIMS_VOLUME_UNIT_TYPE (VOLUME_UNIT_TYPE_CODE, DESCRIPTION)
VALUES
  (N'M3', N'Cubic Meters'),
  (N'F3', N'Cubic Feet');
GO
