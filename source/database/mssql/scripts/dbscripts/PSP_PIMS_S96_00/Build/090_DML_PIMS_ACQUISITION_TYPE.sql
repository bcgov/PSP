/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ACQUISITION_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQUISITION_TYPE
GO

INSERT INTO PIMS_ACQUISITION_TYPE (ACQUISITION_TYPE_CODE, DESCRIPTION)
VALUES
  (N'CONSEN',  N'Consensual Agreement'),
  (N'SECTN3',  N'Section 3 Agreement'),
  (N'SECTN6',  N'Section 6 Expropriation'),
  (N'XFR',     N'Transferred'),
  (N'HISTORY', N'Historical');
GO
