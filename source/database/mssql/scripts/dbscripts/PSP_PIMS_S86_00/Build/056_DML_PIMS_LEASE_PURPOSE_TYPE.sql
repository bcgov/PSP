/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_PURPOSE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version.
Doug Filteau  2024-Apr-15  Added GEOTECH and LNDSRVY.
Doug Filteau  2024-Jul-12  Modification per PSP-8505.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_PURPOSE_TYPE
GO

INSERT INTO PIMS_LEASE_PURPOSE_TYPE (LEASE_PURPOSE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACCCCOM',   N'Access'),
  (N'ACCRES',    N'Other Access'),
  (N'AGRIC',     N'Agricultural Grazing/haying'),
  (N'BCFERRIES', N'BC Ferries'),
  (N'CAMPING',   N'Camping'),
  (N'COMMBLDG',  N'Commercial Building'),
  (N'XING',      N'Crossing'),
  (N'EMERGSVCS', N'Emergency Services'),
  (N'ENCROACH',  N'Encroachment'),
  (N'ENVIRON',   N'Environmental'),
  (N'FENCEGATE', N'Fencing/Gate'),
  (N'GARDENING', N'Garden'),
  (N'GEOTECH',   N'Geotechnical Investigations'),
  (N'GRAVEL',    N'Gravel'),
  (N'GRAZING',   N'Grazing/haying'),
  (N'LNDSRVY',   N'Land Survey'),
  (N'LNDSCPVEG', N'Landscaping/Vegetation Clearing'),
  (N'LOGGING',   N'Logging/Timber Harvest'),
  (N'MTCYARD',   N'Maintenance Yard'),
  (N'MARINEFAC', N'Marine Facility'),
  (N'MOBILEHM',  N'Mobile Home Pad'),
  (N'PARK',      N'Park'),
  (N'PARKNRID',  N'Park and Ride'),
  (N'PARKING',   N'Parking'),
  (N'PIPELINE',  N'Pipeline'),
  (N'PRELOAD',   N'Preload'),
  (N'PRVTRANS',  N'Private transportation'),
  (N'RAILWAY',   N'Railway crossing'),
  (N'RESTAREA',  N'Rest Area/Pullout'),
  (N'SIDEWALK',  N'Sidewalks'),
  (N'SIGNAGE',   N'Signage'),
  (N'SPCLEVNT',  N'Special Event'),
  (N'STGNGAREA', N'Staging Area'),
  (N'STKPILING', N'Stockpiling'),
  (N'STORAGE',   N'Storage'),
  (N'TOURINFO',  N'Tourism'),
  (N'TRAIL',     N'Trail'),
  (N'UTILINFRA', N'Utilities-infrastructure'),
  (N'UTILOHDXG', N'Utilities-overhead xing'),
  (N'UTILUGDXG', N'Utilities-underground xing'),
  (N'WATERRES',  N'Water Reservoir'),
  (N'WEIGHSCL',  N'Weigh Scale'),
  (N'OTHER',     N'Other*'),
  (N'BCTRANSIT', N'BC Transit'),
  (N'TRANSLINK', N'Translink'),
  (N'HOUSING',   N'Housing'),
  (N'HISTORY',   N'Historical'),
  (N'RIPARIAN',  N'Riparian restoration'),
  (N'TRANSITO',  N'Transit Oriented Development (TOD)'),
  (N'REMEDIAL',  N'Remediation'),
  (N'ARCHEOLGY', N'Archeological investigations'),
  (N'RLWYTRSPS', N'Railway trespass');
GO

-- Disable the "LOGGING" type
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'LOGGING'

SELECT LEASE_PURPOSE_TYPE_CODE
FROM   PIMS_LEASE_PURPOSE_TYPE
WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  INSERT INTO PIMS_LEASE_PURPOSE_TYPE (LEASE_PURPOSE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
    VALUES
      (N'LOGGING', N'Logging/Timber Harvest', 1);
ELSE  
  UPDATE PIMS_LEASE_PURPOSE_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PURPOSE_TYPE_CODE = @CurrCd;
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