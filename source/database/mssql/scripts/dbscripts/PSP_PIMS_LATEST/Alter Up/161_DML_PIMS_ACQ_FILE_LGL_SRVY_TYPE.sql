/* -----------------------------------------------------------------------------
Populate the PIMS_ACQ_FILE_LGL_SRVY_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Dec-11  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQ_FILE_LGL_SRVY_TYPE
GO

INSERT INTO PIMS_ACQ_FILE_LGL_SRVY_TYPE (ACQ_FILE_LGL_SRVY_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'NOTREQ',    N'Not required',       1),
  (N'ORDERED',   N'Ordered',            2),
  (N'RECEIVED',  N'Received',           3),
  (N'OUT4SIGN',  N'Out for Signatures', 4),
  (N'REGISTRD',  N'Registered',         5);
GO
