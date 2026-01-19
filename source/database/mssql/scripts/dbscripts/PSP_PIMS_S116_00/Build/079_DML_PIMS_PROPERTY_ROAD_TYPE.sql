/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_ROAD_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_ROAD_TYPE
GO

INSERT INTO PIMS_PROPERTY_ROAD_TYPE (PROPERTY_ROAD_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'CMMNLAW',    N'Common law dedication',             CONVERT([bit],(0))),
  (N'S107PLN',    N'Section 107 Plan',                  CONVERT([bit],(0))),
  (N'107REF',     N'107 Reference Plan',                CONVERT([bit],(1))),
  (N'107EXP',     N'107 Explanatory Plan',              CONVERT([bit],(1))),
  (N'CTRLACC',    N'Controlled Access',                 CONVERT([bit],(0))),
  (N'ARTERIAL',   N'Arterial',                          CONVERT([bit],(0))),
  (N'CRWNDEL',    N'Crown grant deletion',              CONVERT([bit],(0))),
  (N'FEDERAL',    N'Federal',                           CONVERT([bit],(0))),
  (N'FSR',        N'Forest Service Road',               CONVERT([bit],(0))),
  (N'GAZMOTI',    N'Gazetted',                          CONVERT([bit],(0))),
  (N'GAZSURVD',   N'Gazetted (Surveyed)',               CONVERT([bit],(1))),
  (N'GAZUNSURVD', N'Gazetted (Unsurveyed)',             CONVERT([bit],(1))),
  (N'MUNI',       N'Municipal',                         CONVERT([bit],(0))),
  (N'OIC',        N'Order in Council (OIC)',            CONVERT([bit],(0))),
  (N'OTHER',      N'Other',                             CONVERT([bit],(0))),
  (N'PUBRD',      N'Public Road (FLNRORD Juridiction)', CONVERT([bit],(0))),
  (N'S42',        N'S.42 (Court declared)',             CONVERT([bit],(0))),
  (N'S56',        N'S.56 (2) Highway',                  CONVERT([bit],(0))),
  (N'SRWPLAN',    N'SRW plan (with no gazette)',        CONVERT([bit],(0))),
  (N'SUBDIV',     N'Subdivision',                       CONVERT([bit],(0))),
  (N'UNTEN',      N'Untenured (no dedicated highway)',  CONVERT([bit],(0))),
  (N'BYLAW',      N'Bylaw',                             CONVERT([bit],(0)));
GO
