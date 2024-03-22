/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROJECT_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-09  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROJECT_STATUS_TYPE
GO

INSERT INTO PIMS_PROJECT_STATUS_TYPE (PROJECT_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'AC',   N'Active (AC)'),
  (N'CA',   N'Cancelled (CA)'),
  (N'CNCN', N'Consolidated (CNCN)'),
  (N'CO',   N'Completed (CO)'),
  (N'HO',   N'On Hold (HO)'),
  (N'PL',   N'Planning (PL)');
  