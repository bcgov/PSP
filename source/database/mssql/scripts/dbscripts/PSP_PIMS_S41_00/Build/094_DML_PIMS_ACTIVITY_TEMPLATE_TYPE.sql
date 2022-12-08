/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ACTIVITY_TEMPLATE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACTIVITY_TEMPLATE_TYPE
GO

INSERT INTO PIMS_ACTIVITY_TEMPLATE_TYPE (ACTIVITY_TEMPLATE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'GENERAL', N'General'),
  (N'SURVEY',  N'Survey'),
  (N'SITEVIS', N'Site Visit'),
  (N'GENLTR',  N'Generate Letter'),
  (N'FILEDOC', N'File Document');
GO
