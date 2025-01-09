/* -----------------------------------------------------------------------------
Populate the PIMS_ACQ_FILE_EXPROP_RISK_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Dec-11  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQ_FILE_EXPROP_RISK_TYPE
GO

INSERT INTO PIMS_ACQ_FILE_EXPROP_RISK_TYPE (ACQ_FILE_EXPROP_RISK_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'NIL',  N'Nil',    1),
  (N'LOW',  N'Low',    2),
  (N'MED',  N'Medium', 3),
  (N'HIGH', N'High',   4);
GO
