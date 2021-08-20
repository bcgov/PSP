PRINT N'Adding [PIMS_ORGANIZATION_TYPE]'

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
