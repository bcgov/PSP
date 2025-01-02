/* -----------------------------------------------------------------------------
Populate the PIMS_DISPOSITION_FUNDING_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DISPOSITION_FUNDING_TYPE
GO

INSERT INTO PIMS_DISPOSITION_FUNDING_TYPE (DISPOSITION_FUNDING_TYPE_CODE, DESCRIPTION)
VALUES
  (N'BCTFA',     N'BCTFA'),
  (N'CAPPROG',   N'Capital Program'),
  (N'DFAA',      N'DFAA (Disaster Financial Assistance Arrangement)'),
  (N'EVERGREEN', N'Evergreen'),
  (N'FEDERAL',   N'Federal'),
  (N'MAJORPRJ',  N'Major Projects'),
  (N'MOTIDIS',   N'MoTI District'),
  (N'MOTIREG',   N'MoTI Region'),
  (N'OTHER',     N'Other'),
  (N'REHAB',     N'Rehab & Maintenance'),
  (N'TICORP',    N'TI Corp');
GO
