/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_TAKE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_TAKE_STATUS_TYPE
GO

INSERT INTO PIMS_TAKE_STATUS_TYPE (TAKE_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'COMPLETE',   N'Complete'),
  (N'INPROGRESS', N'In-progress'),
  (N'CANCELLED',  N'Cancelled');
GO
