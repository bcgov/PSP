/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_PROGRAM_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version.
Doug Filteau  2024-Jul-12  Many revision per PSP-8505.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_PROGRAM_TYPE
GO

INSERT INTO PIMS_LEASE_PROGRAM_TYPE (LEASE_PROGRAM_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'BCFERRIES', N'BC Ferries',             1),
  (N'BCTRANSIT', N'BC Transit',             1),
  (N'BELLETERM', N'Belleville Terminal',    1),
  (N'TRANSLINK', N'TransLink',              1),
  (N'PUBTRANS',  N'Public Transportation',  0),
  (N'COMMBLDG',  N'Commercial Buildings',   0),
  (N'LCLGOVT',   N'Government',             0),
  (N'OTHER',     N'Other',                  0),
  (N'RAILTRAIL', N'Rail Trails',            0),
  (N'RESRENTAL', N'Residential Rentals',    0),
  (N'TMEP',      N'Oil/Gas',                0),
  (N'UTILITY',   N'Utilities',              0),
  (N'ENGINEER',  N'Engineering',            0),
  (N'MOTIUSE',   N'MoTT Use',               0),
  (N'AGRIC',     N'Agricultural',           0),
  (N'RAIL',      N'Rail',                   0),
  (N'PARKING',   N'Parking and/or Storage', 0);
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE lpty
SET    lpty.DISPLAY_ORDER              = seq.ROW_NUM
     , lpty.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_LEASE_PROGRAM_TYPE lpty JOIN
       (SELECT LEASE_PROGRAM_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_LEASE_PROGRAM_TYPE) seq  ON seq.LEASE_PROGRAM_TYPE_CODE = lpty.LEASE_PROGRAM_TYPE_CODE
GO
