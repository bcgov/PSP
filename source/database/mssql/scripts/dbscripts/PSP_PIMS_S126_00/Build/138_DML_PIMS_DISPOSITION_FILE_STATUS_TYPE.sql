/* -----------------------------------------------------------------------------
Populate the PIMS_DISPOSITION_FILE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Nov-21  Initial version
Doug Filteau  2025-Feb-04  Display order enforced.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DISPOSITION_FILE_STATUS_TYPE
GO

INSERT INTO PIMS_DISPOSITION_FILE_STATUS_TYPE (DISPOSITION_FILE_STATUS_TYPE_CODE, DESCRIPTION, IS_DISABLED, DISPLAY_ORDER)
VALUES
  (N'ACTIVE',    N'Active',    0, 1),
  (N'DRAFT',     N'Draft',     1, 2),
  (N'COMPLETE',  N'Complete',  0, 3),
  (N'HOLD',      N'Hold',      0, 4),
  (N'CANCELLED', N'Cancelled', 0, 5),
  (N'ARCHIVED',  N'Archived',  0, 6);
GO
