/* -----------------------------------------------------------------------------
Populate the PIMS_PROP_MGMT_ACTIVITY_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Sep-11  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROP_MGMT_ACTIVITY_STATUS_TYPE
GO

INSERT INTO PIMS_PROP_MGMT_ACTIVITY_STATUS_TYPE (PROP_MGMT_ACTIVITY_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'NOTSTARTED', N'Not started'),
  (N'INPROGRESS', N'In-progress'),
  (N'COMPLETED',  N'Completed'),
  (N'ONHOLD',     N'On Hold'),
  (N'CANCELLED',  N'Cancelled');
GO
