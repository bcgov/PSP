/* -----------------------------------------------------------------------------
Delete all data from the PIMS_SECURITY_DEPOSIT_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_SECURITY_DEPOSIT_TYPE
GO

INSERT INTO PIMS_SECURITY_DEPOSIT_TYPE (SECURITY_DEPOSIT_TYPE_CODE, DESCRIPTION)
VALUES
  (N'SECURITY', N'Security deposit'),
  (N'PET',      N'Pet deposit'),
  (N'OTHER',    N'Other deposit');