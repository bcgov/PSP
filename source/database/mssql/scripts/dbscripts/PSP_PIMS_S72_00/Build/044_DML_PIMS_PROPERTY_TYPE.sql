/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Mar-09  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_TYPE
GO

INSERT INTO PIMS_PROPERTY_TYPE (PROPERTY_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'LAND',        N'Land',                            CONVERT([bit],(1))),
  (N'BUILD',       N'Buiding',                         CONVERT([bit],(1))),
  (N'SUBDIV',      N'Subdivison',                      CONVERT([bit],(1))),
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