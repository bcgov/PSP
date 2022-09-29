/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_ROAD_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_ROAD_TYPE
GO

INSERT INTO PIMS_PROPERTY_ROAD_TYPE (PROPERTY_ROAD_TYPE_CODE, DESCRIPTION)
VALUES
  (N'GAZSURVD',   N'Gazetted (Surveyed)'),
  (N'GAZUNSURVD', N'Gazetted (Unsurveyed)'),
  (N'GAZMOTI',    N'Gazetted (MoTI Plan)'),
  (N'CRWNDEL',    N'Crown grant deletion'),
  (N'SUBDIV',     N'Subdivision'),
  (N'107REF',     N'107 Reference Plan'),
  (N'107EXP',     N'107 Explanatory Plan'),
  (N'S42',        N'S.42 (Court declared)'),
  (N'S56',        N'S.56 (2) Highway'),
  (N'SRWPLAN',    N'SRW plan (with no gazette)'),
  (N'UNTEN',      N'Untenured (no dedicated highway)'),
  (N'MUNI',       N'Municipal'),
  (N'FSR',        N'Forest Service Road'),
  (N'PUBRD',      N'Public Road (FLNRORD Juridiction)'),
  (N'OIC',        N'Order in Council (OIC)'),
  (N'CTRLACC',    N'Controlled Access'),
  (N'ARTERIAL',   N'Arterial'),
  (N'FEDERAL',    N'Federal'),
  (N'OTHER',      N'Other');
GO
