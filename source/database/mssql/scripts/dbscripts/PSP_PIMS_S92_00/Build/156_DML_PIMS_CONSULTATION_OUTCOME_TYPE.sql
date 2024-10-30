/* -----------------------------------------------------------------------------
Populate the PIMS_CONSULTATION_OUTCOME_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Sep-05  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_CONSULTATION_OUTCOME_TYPE
GO

INSERT INTO PIMS_CONSULTATION_OUTCOME_TYPE (CONSULTATION_OUTCOME_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'INPROGRESS',  N'In-progress',               1),
  (N'APPRGRANTED', N'Approval granted',          2),
  (N'APPRDENIED',  N'Approval denied',           3),
  (N'CONSCOMPLTD', N'Consultation completed',    4),
  (N'CONSDISCONT', N'Consultation discontinued', 5);
GO
