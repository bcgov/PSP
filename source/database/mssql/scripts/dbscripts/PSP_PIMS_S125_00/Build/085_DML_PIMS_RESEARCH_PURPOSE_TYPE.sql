/* -----------------------------------------------------------------------------
Delete all data from the PIMS_RESEARCH_PURPOSE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_RESEARCH_PURPOSE_TYPE
GO

INSERT INTO PIMS_RESEARCH_PURPOSE_TYPE (RESEARCH_PURPOSE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACQUIRE',  N'Acquisition'),
  (N'TENCLEAN', N'Tenure Clean-Up'),
  (N'MGMT',     N'Management'),
  (N'DISPOSE',  N'Disposition'),
  (N'GENENQ',   N'General Enquiry');
GO
