/* -----------------------------------------------------------------------------
Delete all data from the PIMS_REQUEST_SOURCE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_REQUEST_SOURCE_TYPE
GO

INSERT INTO PIMS_REQUEST_SOURCE_TYPE (REQUEST_SOURCE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'PROJECT',  N'Project'),
  (N'NONFOI',   N'Public Enquiry (non-FOI)'),
  (N'FOI',      N'Freedom Of Information (FOI)'),
  (N'DISTRICT', N'District'),
  (N'REGION',   N'Region'),
  (N'HQ',       N'Headquarters (HQ)'),
  (N'OTHERMIN', N'Other Ministry'),
  (N'EXTRQST',  N'External Request'),
  (N'SURVEYOR', N'Surveyor'),
  (N'INTLGL',   N'Legal Counsel (Internal)');
GO
