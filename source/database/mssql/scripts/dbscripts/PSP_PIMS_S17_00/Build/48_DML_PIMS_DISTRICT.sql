/* -----------------------------------------------------------------------------
Delete all data from the PIMS_DISTRICT table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-09  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DISTRICT;
GO

INSERT INTO PIMS_DISTRICT (DISTRICT_CODE, REGION_CODE, DISTRICT_NAME) 
VALUES
  (1, 1, N'Lower Mainland District'),
  (2, 1, N'Vancouver Island District'),
  (3, 2, N'Rocky Mountain District'),
  (4, 2, N'West Kootenay District'),
  (5, 2, N'Okanagan-Shuswap District'),
  (6, 2, N'Thompson-Nicola District'),
  (7, 2, N'Cariboo District'),
  (8, 3, N'Peace District'),
  (9, 3, N'Fort George District'),
  (10, 3, N'Bulkley-Stikine District'),
  (11, 3, N'Skeena District');
GO
