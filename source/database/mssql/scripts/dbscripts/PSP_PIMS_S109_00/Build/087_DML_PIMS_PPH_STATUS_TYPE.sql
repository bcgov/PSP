/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PPH_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version.
Doug Filteau  2025-Jun-30  Added NONE code and DISPLAY_ORDER populated.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PPH_STATUS_TYPE
GO

INSERT INTO PIMS_PPH_STATUS_TYPE (PPH_STATUS_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'ARTERY',  N'Arterial Hwy',                            1),
  (N'COMBO',   N'Combination (PPH & Non-PPH)',             2),
  (N'NONPPH',  N'Non-Provincial Public Highway (Non-PPH)', 3),
  (N'PPH',     N'Provincial Public Highway (PPH)',         4),
  (N'UNKNOWN', N'Unknown',                                98),
  (N'NONE',    N'None',                                   99);
GO
