/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_TAKE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
Doug Filteau  2025-Feb-04  Display order enforced and HOLD added.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_TAKE_STATUS_TYPE
GO

INSERT INTO PIMS_TAKE_STATUS_TYPE (TAKE_STATUS_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'CANCELLED',  N'Cancelled',   1),
  (N'INPROGRESS', N'In-progress', 2),
  (N'COMPLETE',   N'Complete',    3),
  (N'HOLD',       N'Hold',        4);
GO
