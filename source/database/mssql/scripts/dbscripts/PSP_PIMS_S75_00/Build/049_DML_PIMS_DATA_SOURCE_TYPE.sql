/* -----------------------------------------------------------------------------
Delete all data from the PIMS_DATA_SOURCE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version.
Doug Filteau  2024-Mar-22  Added LIS_OPSS_PAIMS_PMBC.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DATA_SOURCE_TYPE
GO

INSERT INTO PIMS_DATA_SOURCE_TYPE (DATA_SOURCE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'OPSS',                N'Operational Spreadsheet'),
  (N'LIS',                 N'Lease Information System (LIS)'),
  (N'PAIMS',               N'Property Acquisition and Inventory Management System (PAIMS)'),
  (N'GAZ',                 N'BC Gazette'),
  (N'PMBC',                N'ParcelMap BC'),
  (N'LIS_OPSS',            N'LIS / OPSS'),
  (N'LIS_PAIMS',           N'LIS / PAIMS'),
  (N'LIS_PMBC',            N'LIS / PMBC'),
  (N'OPSS_PAIMS',          N'OPSS / PAIMS'),
  (N'PAIMS_PMBC',          N'PAIMS / PMBC'),
  (N'LIS_PAIMS_PMBC',      N'LIS / PAIMS / PMBC'),
  (N'LIS_OPSS_PAIMS',      N'LIS / OPSS / PAIMS'),
  (N'LIS_OPSS_PAIMS_PMBC', N'LIS / OPSS / PAIMS / PMBC');
GO
