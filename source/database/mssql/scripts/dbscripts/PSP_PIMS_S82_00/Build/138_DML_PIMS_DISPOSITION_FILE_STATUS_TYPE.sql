/* -----------------------------------------------------------------------------
Populate the PIMS_DISPOSITION_FILE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Nov-21  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DISPOSITION_FILE_STATUS_TYPE
GO

INSERT INTO PIMS_DISPOSITION_FILE_STATUS_TYPE (DISPOSITION_FILE_STATUS_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'ACTIVE',    N'Active',    0),
  (N'ARCHIVED',  N'Archived',  0),
  (N'CANCELLED', N'Cancelled', 0),
  (N'COMPLETE',  N'Complete',  0),
  (N'DRAFT',     N'Draft',     1),
  (N'HOLD',      N'Hold',      0);
GO
