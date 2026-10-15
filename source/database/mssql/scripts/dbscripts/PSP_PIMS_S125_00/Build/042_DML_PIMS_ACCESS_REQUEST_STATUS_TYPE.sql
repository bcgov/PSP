/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ACCESS_REQUEST_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACCESS_REQUEST_STATUS_TYPE
GO

INSERT INTO PIMS_ACCESS_REQUEST_STATUS_TYPE (ACCESS_REQUEST_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'INITIATED', N'Access Request initiated but receipt not acknowedged'),
  (N'RECEIVED', N'Access Request confirmed received'),
  (N'UNDERREVIEW', N'Access Request under review'),
  (N'REVIEWCOMPLETE', N'Access Request review compleeted'),
  (N'MOREINFO', N'Access Request requires additional information'),
  (N'APPROVED', N'Access Request approved'),
  (N'DENIED', N'Access Request denied');