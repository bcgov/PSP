PRINT N'Inserting [PIMS_LEASE_PROGRAM_TYPE]'
GO
INSERT INTO PIMS_LEASE_PROGRAM_TYPE (LEASE_PROGRAM_TYPE_CODE, DESCRIPTION)
VALUES
  (N'BCFERRIES', N'BC Ferries'),
  (N'BCTRANSIT', N'BC Transit'),
  (N'BELLETERM', N'Belleville Terminal'),
  (N'COMMBLDG', N'Commercial Buildings'),
  (N'LCLGOVT', N'Local Government/RCMP'),
  (N'OTHER', N'Other Licencing'),
  (N'RAILTRAIL', N'Rail Trails'),
  (N'RESRENTAL', N'Residential Rentals'),
  (N'TMEP', N'TMEP'),
  (N'TRANSLINK', N'TransLink'),
  (N'UTILITY', N'Utilities');
