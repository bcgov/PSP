/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_PURPOSE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
Doug Filteau  2024-Apr-15  Added GEOTECH and LNDSRVY.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_PURPOSE_TYPE
GO

INSERT INTO PIMS_LEASE_PURPOSE_TYPE (LEASE_PURPOSE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'ACCCCOM',   N'Access (Industrial/Commercial)',   1),
  (N'ACCRES',    N'Access (Residential)',             2),
  (N'AGRIC',     N'Agricultural',                     3),
  (N'BCFERRIES', N'BC Ferries',                       4),
  (N'CAMPING',   N'Camping',                          5),
  (N'COMMBLDG',  N'Commercial Building',              6),
  (N'XING',      N'Crossing',                         7),
  (N'EMERGSVCS', N'Emergency Services',               8),
  (N'ENCROACH',  N'Encroachment',                     9),
  (N'ENVIRON',   N'Environmental',                   10),
  (N'FENCEGATE', N'Fencing/Gate',                    11),
  (N'GARDENING', N'Garden',                          12),
  (N'GEOTECH',   N'Geotechnical Investigations',     13),
  (N'GRAVEL',    N'Gravel',                          14),
  (N'GRAZING',   N'Grazing',                         15),
  (N'LNDSRVY',   N'Land Survey',                     16),
  (N'LNDSCPVEG', N'Landscaping/Vegetation Clearing', 17),
  (N'LOGGING',   N'Logging/Timber Harvest',          18),
  (N'MTCYARD',   N'Maintenance Yard',                19),
  (N'MARINEFAC', N'Marine Facility',                 20),
  (N'MOBILEHM',  N'Mobile Home Pad',                 21),
  (N'PARK',      N'Park',                            22),
  (N'PARKNRID',  N'Park and Ride',                   23),
  (N'PARKING',   N'Parking',                         24),
  (N'PIPELINE',  N'Pipeline',                        25),
  (N'PRELOAD',   N'Preload',                         26),
  (N'PRVTRANS',  N'Private transportation',          27),
  (N'RAILWAY',   N'Railway',                         28),
  (N'RESTAREA',  N'Rest Area/Pullout',               29),
  (N'SIDEWALK',  N'Sidewalks',                       30),
  (N'SIGNAGE',   N'Signage',                         31),
  (N'SPCLEVNT',  N'Special Event',                   32),
  (N'STGNGAREA', N'Staging Area',                    33),
  (N'STKPILING', N'Stockpiling',                     34),
  (N'STORAGE',   N'Storage',                         35),
  (N'TOURINFO',  N'Tourist Info',                    36),
  (N'TRAIL',     N'Trail',                           37),
  (N'UTILINFRA', N'Utilities-infrastructure',        38),
  (N'UTILOHDXG', N'Utilities-overhead xing',         39),
  (N'UTILUGDXG', N'Utilities-underground xing',      40),
  (N'WATERRES',  N'Water Reservoir',                 41),
  (N'WEIGHSCL',  N'Weigh Scale',                     42),
  (N'OTHER',     N'Other*',                          99);
  GO
  