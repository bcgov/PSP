/* -----------------------------------------------------------------------------
Delete all data from the PIMS_TASK_TEMPLATE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_TASK_TEMPLATE_TYPE
GO

INSERT INTO PIMS_TASK_TEMPLATE_TYPE (TASK_TEMPLATE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'CONFIRMOWNER', N'Confirm property ownership'),
  (N'ASSESSVALUE', N'Assess property value'),
  (N'SITEVISIT', N'Perform site visit'),
  (N'CONFIRMBOUNDS', N'Confirm property boundaries');