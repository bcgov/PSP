/* -----------------------------------------------------------------------------
Delete all data from the PIMS_RESEARCH_FILE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_RESEARCH_FILE_STATUS_TYPE
GO

INSERT INTO PIMS_RESEARCH_FILE_STATUS_TYPE (RESEARCH_FILE_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACTIVE',   N'Active'),
  (N'INACTIVE', N'Inactive'),
  (N'CLOSED',   N'Closed'),
  (N'ARCHIVED', N'Archived');
GO
