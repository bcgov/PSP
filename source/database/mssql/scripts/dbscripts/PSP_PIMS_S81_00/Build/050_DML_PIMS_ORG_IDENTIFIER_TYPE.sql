/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ORG_IDENTIFIER_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ORG_IDENTIFIER_TYPE
GO

INSERT INTO PIMS_ORG_IDENTIFIER_TYPE (ORG_IDENTIFIER_TYPE_CODE, DESCRIPTION)
VALUES
  (N'BCeIDBUSINESS', N'A BC electronic identifier that allows businesses to connect to the BC Government to manage a variety of company information'),
  (N'GUID', N'A global unique identifier for an Organization that is a user of the Property Information Management System'),
  (N'GOV', N'A Government organization identifier'),
  (N'OTHINCORPNO', N'Other form of identifier for an Organization');