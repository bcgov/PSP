/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LAND_SURVEYOR_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LAND_SURVEYOR_TYPE
GO

INSERT INTO PIMS_LAND_SURVEYOR_TYPE (LAND_SURVEYOR_TYPE_CODE, DESCRIPTION)
VALUES
  (N'CLS',   N'CLS (Canada Land Surveyor)'),
  (N'BCLS',  N'BCLS (BC Land Surveyor)'),
  (N'OTHER', N'Other');
GO
