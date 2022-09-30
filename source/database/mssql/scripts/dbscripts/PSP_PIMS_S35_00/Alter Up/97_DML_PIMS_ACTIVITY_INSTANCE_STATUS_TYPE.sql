/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ACTIVITY_INSTANCE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

--DELETE FROM PIMS_ACTIVITY_INSTANCE_STATUS_TYPE
--GO

INSERT INTO PIMS_ACTIVITY_INSTANCE_STATUS_TYPE (ACTIVITY_INSTANCE_STATUS_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
--  (N'NOSTART',   N'Not Started', 1),
  (N'INPROG',    N'In Progress', 2),
  (N'COMPLETE',  N'Complete',    3),
  (N'CANCELLED', N'Cancelled',   4);
GO
