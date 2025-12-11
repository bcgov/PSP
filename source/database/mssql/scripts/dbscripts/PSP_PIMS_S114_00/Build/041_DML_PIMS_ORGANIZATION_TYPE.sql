/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ORGANIZATION_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ORGANIZATION_TYPE
GO

INSERT INTO PIMS_ORGANIZATION_TYPE (ORGANIZATION_TYPE_CODE, DESCRIPTION)
VALUES
  (N'FIRSTNAT', N'First Nations'),
  (N'REALTOR', N'Real estate corporation'),
  (N'PRIVATE', N'Private owner'),
  (N'RAILWAY', N'Railway corporation'),
  (N'OTHER', N'Other'),
  (N'BCMIN', N'BC Ministry'),
  (N'BCREG', N'BC Regional Office'),
  (N'BCDIST', N'BC District Office'),
  (N'PARTNER', N'Partnership');