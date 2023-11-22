/* -----------------------------------------------------------------------------
Populate the PIMS_DSP_FL_TEAM_PROFILE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DSP_FL_TEAM_PROFILE_TYPE
GO

INSERT INTO PIMS_DSP_FL_TEAM_PROFILE_TYPE (DSP_FL_TEAM_PROFILE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'BCTFA',     N'BCTFA'),
  (N'CAPPROG',   N'Capital Program'),
  (N'DFAA',      N'DFAA (Disaster Financial Assistance Arrangement)'),
  (N'EVERGREEN', N'Evergreen'),
  (N'FEDERAL',   N'Federal'),
  (N'MAJORPRJ',  N'Major Projects'),
  (N'MoTIDis',   N'MoTI District'),
  (N'MoTIReg',   N'MoTI Region'),
  (N'Other',     N'Other'),
  (N'Rehab',     N'Rehab & Maintenance'),
  (N'TICorp',    N'TI Corp');
GO
