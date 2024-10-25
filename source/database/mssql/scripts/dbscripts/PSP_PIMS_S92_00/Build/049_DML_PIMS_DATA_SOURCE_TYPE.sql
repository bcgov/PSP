/* -----------------------------------------------------------------------------
Delete all data from the PIMS_DATA_SOURCE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version.
Doug Filteau  2024-Mar-22  Added LIS_OPSS_PAIMS_PMBC.
Doug Filteau  2024-Oct-17  Added PAT, TAP, BIP, GWP, and SHAREPOINT.
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
  (N'LIS_OPSS_PAIMS_PMBC', N'LIS / OPSS / PAIMS / PMBC'),
  (N'PIMS',                N'PIMS'),
  (N'PAT',                 N'PAT'),
  (N'TAP',                 N'TAP'),
  (N'SHAREPOINT',          N'SharePoint'),
  (N'BIP',                 N'BIP'),
  (N'GWP',                 N'GWP');
GO

-- --------------------------------------------------------------
-- Update the display order with the exception of the OTHER type.
-- --------------------------------------------------------------
UPDATE prnt
SET    prnt.DISPLAY_ORDER              = chld.ROW_NUM
     , prnt.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_DATA_SOURCE_TYPE prnt JOIN
       (SELECT DATA_SOURCE_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_DATA_SOURCE_TYPE) chld ON chld.DATA_SOURCE_TYPE_CODE = prnt.DATA_SOURCE_TYPE_CODE
GO
