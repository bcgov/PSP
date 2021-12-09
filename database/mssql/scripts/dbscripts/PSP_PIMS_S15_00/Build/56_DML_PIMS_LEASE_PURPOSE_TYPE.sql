/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_PURPOSE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_PURPOSE_TYPE
GO

INSERT INTO PIMS_LEASE_PURPOSE_TYPE (LEASE_PURPOSE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACCRES', N'Access (Residential)'),
  (N'ACCCCOM', N'Access (Industrial/Commercial)'),
  (N'AGRIC', N'Agricultural'),
  (N'COMMBLDG', N'Commercial Building'),
  (N'CAMPING', N'Camping'),
  (N'XING', N'Crossing'),
  (N'ENCROACH', N'Encroachment'),
  (N'FENCEGATE', N'Fencing/Gate'),
  (N'MARINEFAC', N'Marine Facility'),
  (N'EMERGSVCS', N'Emergency Services'),
  (N'BCFERRIES', N'BC Ferries'),
  (N'GARDENING', N'Gardening'),
  (N'GRAVEL', N'Gravel'),
  (N'GRAZING', N'Grazing'),
  (N'SIDEWALK', N'Sidewalks/Landscaping'),
  (N'LOGGING', N'Logging/Timber Harvest'),
  (N'MTCYARD', N'Maintenance Yard'),
  (N'MOBILEHM', N'Mobile Home Pad'),
  (N'OTHER', N'Other*'),
  (N'PARK', N'Park'),
  (N'PARKNRID', N'Park and Ride'),
  (N'PARKING', N'Parking'),
  (N'PIPELINE', N'Pipeline'),
  (N'PRVTRANS', N'Private transportation'),
  (N'PRELOAD', N'Preload'),
  (N'RAILWAY', N'Railway'),
  (N'RESTAREA', N'Rest Area/Pullout'),
  (N'SIGNAGE', N'Signage'),
  (N'SPCLEVNT', N'Special Event'),
  (N'STGNGAREA', N'Staging Area'),
  (N'STKPILING', N'Stockpiling'),
  (N'STORAGE', N'Storage'),
  (N'TOURINFO', N'Tourist Info'),
  (N'TRAIL', N'Trail'),
  (N'UTILINFRA', N'Utilities-infrastructure'),
  (N'UTILOHDXG', N'Utilities-overhead xing'),
  (N'UTILUGDXG', N'Utilities-underground xing'),
  (N'WATERRES', N'Water Reservoir'),
  (N'WEIGHSCL', N'Weigh Scale');