/* -----------------------------------------------------------------------------
Insert data into the PIMS_ACTIVITY_TEMPLATE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Sep-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACTIVITY_TEMPLATE
GO

INSERT INTO PIMS_ACTIVITY_TEMPLATE (ACTIVITY_TEMPLATE_TYPE_CODE, CONCURRENCY_CONTROL_NUMBER)
  VALUES (N'GENERAL', 1)
GO
