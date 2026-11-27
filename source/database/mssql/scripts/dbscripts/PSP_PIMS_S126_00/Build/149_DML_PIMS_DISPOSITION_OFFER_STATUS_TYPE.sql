/* -----------------------------------------------------------------------------
Populate the PIMS_DISPOSITION_OFFER_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Dec-05  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DISPOSITION_OFFER_STATUS_TYPE
GO

INSERT INTO PIMS_DISPOSITION_OFFER_STATUS_TYPE (DISPOSITION_OFFER_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'OPEN',      N'Open'),
  (N'REJECTED',  N'Rejected'),
  (N'COUNTERED', N'Countered'),
  (N'ACCCEPTED', N'Accepted'),
  (N'COLLAPSED', N'Collapsed');
GO
