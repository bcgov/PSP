/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_TYPE
GO

INSERT INTO PIMS_LEASE_TYPE (LEASE_TYPE_CODE, DESCRIPTION)
VALUES
  (5100, N'Marketing Fee'),
  (5120, N'Advertising'),
  (5140, N'Cleaning'),
  (5160, N'Security'),
  (5180, N'BC Hydro'),
  (5200, N'Landscaping'),
  (5220, N'BC Gas'),
  (5240, N'Carpentry'),
  (5260, N'Legal'),
  (5280, N'Maintenance'),
  (5300, N'Management'),
  (5320, N'Miscellaneous'),
  (5340, N'Painting'),
  (5360, N'Plumbing/Heating'),
  (5380, N'Legal Survey'),
  (5390, N'Report/Consulting Fee'),
  (5400, N'Property Purchase Tax'),
  (5420, N'Commission Fee'),
  (5440, N'Level 1 Assessment'),
  (5460, N'Property Taxes'),
  (5500, N'Rubbish Removal'),
  (5520, N'Water/Sewer'),
  (6100, N'Security Deposit'),
  (6120, N'Rental/Lease Revenue'),
  (6140, N'Other Income');