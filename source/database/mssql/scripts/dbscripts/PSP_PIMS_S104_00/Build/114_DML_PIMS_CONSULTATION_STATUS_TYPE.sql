/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_CONSULTATION_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_CONSULTATION_STATUS_TYPE
GO

INSERT INTO PIMS_CONSULTATION_STATUS_TYPE (CONSULTATION_STATUS_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'UNKNOWN',  N'Unknown',                 1),
  (N'REQNTCOM', N'Required, not completed', 2),
  (N'REQCOMP',  N'Required, completed',     3),
  (N'NOTREQD',  N'Not required',            4);
GO
