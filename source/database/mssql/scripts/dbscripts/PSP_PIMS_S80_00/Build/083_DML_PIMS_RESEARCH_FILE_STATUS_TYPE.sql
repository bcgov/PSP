/* -----------------------------------------------------------------------------
Delete all data from the PIMS_RESEARCH_FILE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
Doug Filteau  2024-May-02  Renamed "Closed" to "Complete" and renamed "Inactive" 
                           to "Hold"
----------------------------------------------------------------------------- */

DELETE FROM PIMS_RESEARCH_FILE_STATUS_TYPE
GO

INSERT INTO PIMS_RESEARCH_FILE_STATUS_TYPE (RESEARCH_FILE_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACTIVE',   N'Active'),
  (N'INACTIVE', N'Hold'),
  (N'CLOSED',   N'Complete'),
  (N'ARCHIVED', N'Archived');
GO
