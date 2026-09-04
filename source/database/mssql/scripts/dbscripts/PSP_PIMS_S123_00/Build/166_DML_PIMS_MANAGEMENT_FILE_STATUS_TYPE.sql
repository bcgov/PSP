/* -----------------------------------------------------------------------------
Populate the PIMS_MANAGEMENT_FILE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Apr-11  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_MANAGEMENT_FILE_STATUS_TYPE
GO

INSERT INTO PIMS_MANAGEMENT_FILE_STATUS_TYPE (MANAGEMENT_FILE_STATUS_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'ACTIVE',    N'Active',         1),
  (N'DRAFT',     N'Draft',          2),
  (N'HOLD',      N'Hold',           3),
  (N'CANCELLED', N'Cancelled',      4),
  (N'COMPLETE',  N'Complete',       5),
  (N'3RDPARTY',  N'With 3rd Party', 6),
  (N'ARCHIVED',  N'Archived',       7);
GO
