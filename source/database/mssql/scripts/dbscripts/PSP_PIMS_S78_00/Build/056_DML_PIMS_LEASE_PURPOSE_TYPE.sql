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
  (N'ACCRES',    N'Access (Residential)',            1),
  (N'ACCCCOM',   N'Access (Industrial/Commercial)',  2),
  (N'AGRIC',     N'Agricultural',                    3),
  (N'COMMBLDG',  N'Commercial Building',             4),
  (N'CAMPING',   N'Camping',                         5),
  (N'XING',      N'Crossing',                        6),
  (N'ENCROACH',  N'Encroachment',                    7),
  (N'FENCEGATE', N'Fencing/Gate',                    8),
  (N'MARINEFAC', N'Marine Facility',                 9),
  (N'EMERGSVCS', N'Emergency Services',             10),
  (N'BCFERRIES', N'BC Ferries',                     11),
  (N'GARDENING', N'Gardening',                      12),
  (N'GEOTECH',   N'Geotechnical Investigations',    13),
  (N'GRAVEL',    N'Gravel',                         14),
  (N'GRAZING',   N'Grazing',                        15),
  (N'SIDEWALK',  N'Sidewalks/Landscaping',          16),
  (N'LNDSRVY',   N'Land Survey Purposes',           17),
  (N'LOGGING',   N'Logging/Timber Harvest',         18),
  (N'MTCYARD',   N'Maintenance Yard',               19),
  (N'MOBILEHM',  N'Mobile Home Pad',                20),
  (N'PARK',      N'Park',                           21),
  (N'PARKNRID',  N'Park and Ride',                  22),
  (N'PARKING',   N'Parking',                        23),
  (N'PIPELINE',  N'Pipeline',                       24),
  (N'PRVTRANS',  N'Private transportation',         25),
  (N'PRELOAD',   N'Preload',                        26),
  (N'RAILWAY',   N'Railway',                        27),
  (N'RESTAREA',  N'Rest Area/Pullout',              28),
  (N'SIGNAGE',   N'Signage',                        29),
  (N'SPCLEVNT',  N'Special Event',                  30),
  (N'STGNGAREA', N'Staging Area',                   31),
  (N'STKPILING', N'Stockpiling',                    32),
  (N'STORAGE',   N'Storage',                        33),
  (N'TOURINFO',  N'Tourist Info',                   34),
  (N'TRAIL',     N'Trail',                          35),
  (N'UTILINFRA', N'Utilities-infrastructure',       36),
  (N'UTILOHDXG', N'Utilities-overhead xing',        37),
  (N'UTILUGDXG', N'Utilities-underground xing',     38),
  (N'WATERRES',  N'Water Reservoir',                39),
  (N'WEIGHSCL',  N'Weigh Scale',                    40),
  (N'OTHER',     N'Other*',                         99);
  GO
  