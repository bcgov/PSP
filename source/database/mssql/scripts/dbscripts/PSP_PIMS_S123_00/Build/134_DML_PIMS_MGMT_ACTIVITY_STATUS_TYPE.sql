/* -----------------------------------------------------------------------------
Populate the PIMS_MGMT_ACTIVITY_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Sep-11  Initial version
Doug Filteau  2025-Jul-17  PIMS_PROP_MGMT_ACTIVITY_STATUS_TYPE table renamed to 
                           PIMS_MGMT_ACTIVITY_STATUS_TYPE.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_MGMT_ACTIVITY_STATUS_TYPE
GO

INSERT INTO PIMS_MGMT_ACTIVITY_STATUS_TYPE (MGMT_ACTIVITY_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'NOTSTARTED', N'Not started'),
  (N'INPROGRESS', N'In-progress'),
  (N'COMPLETED',  N'Completed'),
  (N'ONHOLD',     N'On Hold'),
  (N'CANCELLED',  N'Cancelled');
GO
