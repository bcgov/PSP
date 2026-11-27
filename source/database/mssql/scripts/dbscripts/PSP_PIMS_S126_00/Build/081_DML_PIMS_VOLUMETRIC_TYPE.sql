/* -----------------------------------------------------------------------------
Delete all data from the PIMS_VOLUMETRIC_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_VOLUMETRIC_TYPE
GO

INSERT INTO PIMS_VOLUMETRIC_TYPE (VOLUMETRIC_TYPE_CODE, DESCRIPTION)
VALUES
  (N'AIRSPACE',  N'Airspace'),
  (N'SUBSURF',   N'Sub-surface');
GO
