/* -----------------------------------------------------------------------------
Populate the PIMS_DISPOSITION_FILE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Nov-21  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DISPOSITION_FILE_STATUS_TYPE
GO

INSERT INTO PIMS_DISPOSITION_FILE_STATUS_TYPE (DISPOSITION_FILE_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACTIVE',    N'Active'),
  (N'ARCHIVED',  N'Archived'),
  (N'CANCELLED', N'Cancelled'),
  (N'COMPLETE',  N'Complete'),
  (N'DRAFT',     N'Draft'),
  (N'HOLD',      N'Hold');
GO
