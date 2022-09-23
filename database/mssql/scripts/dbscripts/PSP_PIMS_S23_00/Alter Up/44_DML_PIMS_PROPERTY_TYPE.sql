/* -----------------------------------------------------------------------------
Disable existing codes in the PIMS_PROPERTY_TYPE table and add new codes.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Mar-09  Initial version
----------------------------------------------------------------------------- */
  
BEGIN TRANSACTION;

-- Disable the existing code values
UPDATE PIMS_PROPERTY_TYPE
SET    IS_DISABLED                = CONVERT([bit],(1))
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1;

-- If existing code values not found, insert the disabled legacy codes to 
-- support legacy data.
IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_PROPERTY_TYPE (PROPERTY_TYPE_CODE, DESCRIPTION, IS_DISABLED)
    VALUES
	  (N'LAND',   N'Land',       CONVERT([bit],(1))),
	  (N'BUILD',  N'Buiding',    CONVERT([bit],(1))),
	  (N'SUBDIV', N'Subdivison', CONVERT([bit],(1)));
  END

-- Insert the new code values
INSERT INTO PIMS_PROPERTY_TYPE (PROPERTY_TYPE_CODE, DESCRIPTION, IS_DISABLED)
  VALUES
    (N'TITLED',      N'Titled',                          CONVERT([bit],(0))),
    (N'HWYROAD',     N'Hwy / Road',                      CONVERT([bit],(0))),
    (N'STRATACP',    N'Strata Common Property',          CONVERT([bit],(0))),
    (N'CROWNPSRVD',  N'Crown - Provincial - Surveyed',   CONVERT([bit],(0))),
    (N'CROWNPUSRVD', N'Crown - Provincial - Unsurveyed', CONVERT([bit],(0))),
    (N'CROWNFSRVD',  N'Crown - Federal - Surveyed',      CONVERT([bit],(0))),
    (N'CROWNFUSRVD', N'Crown - Federal - Unsurveyed',    CONVERT([bit],(0))),
    (N'RESERVE',     N'Reserve (IR)',                    CONVERT([bit],(0))),
    (N'PARKS',       N'Parks',                           CONVERT([bit],(0))),
    (N'UNKNOWN',     N'Unknown',                         CONVERT([bit],(0)));

COMMIT TRANSACTION;
