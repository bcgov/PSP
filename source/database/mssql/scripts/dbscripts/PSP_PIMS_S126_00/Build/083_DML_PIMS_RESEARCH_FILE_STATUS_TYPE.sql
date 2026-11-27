/* -----------------------------------------------------------------------------
Delete all data from the PIMS_RESEARCH_FILE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
Doug Filteau  2024-May-02  Renamed "Closed" to "Complete" and renamed "Inactive" 
                           to "Hold"
Doug Filteau  2025-Feb-04  Display order enforced
----------------------------------------------------------------------------- */

DELETE FROM PIMS_RESEARCH_FILE_STATUS_TYPE
GO

INSERT INTO PIMS_RESEARCH_FILE_STATUS_TYPE (RESEARCH_FILE_STATUS_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'ACTIVE',   N'Active',   1),
  (N'CLOSED',   N'Complete', 2),
  (N'INACTIVE', N'Hold',     3),
  (N'ARCHIVED', N'Archived', 4);
GO
