/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROJECT_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-09  Initial version
Doug Filteau  2025-Feb-04  Display order enforced
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROJECT_STATUS_TYPE
GO

INSERT INTO PIMS_PROJECT_STATUS_TYPE (PROJECT_STATUS_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'AC',   N'Active (AC)',         1),
  (N'CO',   N'Completed (CO)',      2),
  (N'HO',   N'On Hold (HO)',        3),
  (N'PL',   N'Planning (PL)',       4),
  (N'CA',   N'Cancelled (CA)',      5),
  (N'CNCN', N'Consolidated (CNCN)', 6);
  GO
