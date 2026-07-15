/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ACQUISITION_FILE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQUISITION_FILE_STATUS_TYPE
GO

INSERT INTO PIMS_ACQUISITION_FILE_STATUS_TYPE (ACQUISITION_FILE_STATUS_TYPE_CODE, DESCRIPTION, IS_DISABLED, DISPLAY_ORDER)
VALUES
  (N'ACTIVE', N'Active',    0, 1),
  (N'DRAFT',  N'Draft',     0, 2),
  (N'COMPLT', N'Complete',  0, 3),
  (N'HOLD',   N'Hold',      0, 4),
  (N'CANCEL', N'Cancelled', 0, 5),
  (N'ARCHIV', N'Archived',  0, 6),
  (N'CLOSED', N'Closed',    1, 7);
GO
