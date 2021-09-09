PRINT N'Inserting [PIMS_LEASE_PURPOSE_TYPE]'
GO
INSERT INTO PIMS_LEASE_PURPOSE_TYPE (LEASE_PURPOSE_TYPE_CODE, DESCRIPTION)
VALUES
  (1, N'Residential'),
  (2, N'Commercial'),
  (3, N'Industrial'),
  (4, N'Nominal');
