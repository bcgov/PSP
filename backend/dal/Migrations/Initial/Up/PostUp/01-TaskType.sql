PRINT N'Adding [PIMS_TASK_TEMPLATE_TYPE]'

INSERT INTO PIMS_TASK_TEMPLATE_TYPE (TASK_TEMPLATE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'CONFIRMOWNER', N'Confirm property ownership'),
  (N'ASSESSVALUE', N'Assess property value'),
  (N'SITEVISIT', N'Perform site visit'),
  (N'CONFIRMBOUNDS', N'Confirm property boundaries');
