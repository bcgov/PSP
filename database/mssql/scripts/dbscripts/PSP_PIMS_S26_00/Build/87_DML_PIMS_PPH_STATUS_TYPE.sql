/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PPH_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PPH_STATUS_TYPE
GO

INSERT INTO PIMS_PPH_STATUS_TYPE (PPH_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'PPH',     N'Provincial Public Highway (PPH)'),
  (N'NONPPH',  N'Non-Provincial Public Highway (Non-PPH)'),
  (N'COMBO',   N'Combination (PPH & Non-PPH'),
  (N'UNKNOWN', N'Unknown');
GO
