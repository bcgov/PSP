/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_PURPOSE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version.
Doug Filteau  2024-Apr-15  Added GEOTECH and LNDSRVY.
Doug Filteau  2024-Jul-12  Modification per PSP-8505.
Doug Filteau  2024-Oct-07  Disable ACCRES.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_PURPOSE_TYPE
GO

INSERT INTO PIMS_LEASE_PURPOSE_TYPE (LEASE_PURPOSE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'ACCCCOM',   N'Access',                             0),
  (N'ACCRES',    N'Other Access',                       1),
  (N'AGRIC',     N'Agricultural Grazing/haying',        1),
  (N'BCFERRIES', N'BC Ferries',                         0),
  (N'CAMPING',   N'Camping',                            0),
  (N'COMMBLDG',  N'Commercial Building',                0),
  (N'XING',      N'Crossing',                           0),
  (N'EMERGSVCS', N'Emergency Services',                 0),
  (N'ENCROACH',  N'Encroachment',                       0),
  (N'ENVIRON',   N'Environmental',                      0),
  (N'FENCEGATE', N'Fencing/Gate',                       0),
  (N'GARDENING', N'Garden',                             0),
  (N'GEOTECH',   N'Geotechnical Investigations',        0),
  (N'GRAVEL',    N'Gravel',                             0),
  (N'GRAZING',   N'Grazing/haying',                     0),
  (N'LNDSRVY',   N'Land Survey',                        0),
  (N'LNDSCPVEG', N'Landscaping/Vegetation Clearing',    0),
  (N'LOGGING',   N'Logging/Timber Harvest',             1),
  (N'MTCYARD',   N'Maintenance Yard',                   0),
  (N'MARINEFAC', N'Marine Facility',                    0),
  (N'MOBILEHM',  N'Mobile Home Pad',                    0),
  (N'PARK',      N'Park',                               0),
  (N'PARKNRID',  N'Park and Ride',                      0),
  (N'PARKING',   N'Parking',                            0),
  (N'PIPELINE',  N'Pipeline',                           0),
  (N'PRELOAD',   N'Preload',                            0),
  (N'PRVTRANS',  N'Private transportation',             0),
  (N'RAILWAY',   N'Railway crossing',                   0),
  (N'RESTAREA',  N'Rest Area/Pullout',                  0),
  (N'SIDEWALK',  N'Sidewalks',                          0),
  (N'SIGNAGE',   N'Signage',                            0),
  (N'SPCLEVNT',  N'Special Event',                      0),
  (N'STGNGAREA', N'Staging Area',                       0),
  (N'STKPILING', N'Stockpiling',                        0),
  (N'STORAGE',   N'Storage',                            0),
  (N'TOURINFO',  N'Tourism',                            0),
  (N'TRAIL',     N'Trail',                              0),
  (N'UTILINFRA', N'Utilities-infrastructure',           0),
  (N'UTILOHDXG', N'Utilities-overhead xing',            0),
  (N'UTILUGDXG', N'Utilities-underground xing',         0),
  (N'WATERRES',  N'Water Reservoir',                    0),
  (N'WEIGHSCL',  N'Weigh Scale',                        0),
  (N'OTHER',     N'Other*',                             0),
  (N'BCTRANSIT', N'BC Transit',                         0),
  (N'TRANSLINK', N'Translink',                          0),
  (N'HOUSING',   N'Housing',                            0),
  (N'HISTORY',   N'Historical',                         0),
  (N'RIPARIAN',  N'Riparian restoration',               0),
  (N'TRANSITO',  N'Transit Oriented Development (TOD)', 0),
  (N'REMEDIAL',  N'Remediation',                        0),
  (N'ARCHEOLGY', N'Archeological investigations',       0),
  (N'RLWYTRSPS', N'Railway trespass',                   0);
GO

-- --------------------------------------------------------------
-- Update the display order with the exception of the OTHER type.
-- --------------------------------------------------------------
UPDATE tbl
SET    tbl.DISPLAY_ORDER              = seq.ROW_NUM
     , tbl.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_LEASE_PURPOSE_TYPE tbl JOIN
       (SELECT LEASE_PURPOSE_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_LEASE_PURPOSE_TYPE
        WHERE  LEASE_PURPOSE_TYPE_CODE <> N'OTHER') seq  ON seq.LEASE_PURPOSE_TYPE_CODE = tbl.LEASE_PURPOSE_TYPE_CODE
WHERE  tbl.LEASE_PURPOSE_TYPE_CODE <> N'OTHER'
GO

-- --------------------------------------------------------------
-- Set the OTHER type to always appear at the end of the list.
-- --------------------------------------------------------------
UPDATE PIMS_LEASE_PURPOSE_TYPE
SET    DISPLAY_ORDER              = 99
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_PURPOSE_TYPE_CODE = N'OTHER'
GO
