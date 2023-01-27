/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ACQUISITION_FUNDING_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQUISITION_FUNDING_TYPE
GO

INSERT INTO PIMS_ACQUISITION_FUNDING_TYPE (ACQUISITION_FUNDING_TYPE_CODE, DESCRIPTION)
VALUES
  (N'MOTIDST', N'MoTI District'),
  (N'MOTIREG', N'MoTI Region'),
  (N'DFAA',    N'DFAA (Disaster Financial Assistance Arrangement)'),
  (N'REHAB',   N'Rehab & Maintenance'),
  (N'CAPITAL', N'Capital Program'),
  (N'BCTFA',   N'BCTFA'),
  (N'TICORP',  N'TI Corp'),
  (N'MAJORPR', N'Major Projects'),
  (N'FEDERAL', N'Federal'),
  (N'EVERGRN', N'Evergreen'),
  (N'OTHER',   N'Other');
GO
