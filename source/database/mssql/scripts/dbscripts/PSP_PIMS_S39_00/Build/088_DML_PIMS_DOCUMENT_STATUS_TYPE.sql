/* -----------------------------------------------------------------------------
Delete all data from the PIMS_DOCUMENT_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DOCUMENT_STATUS_TYPE
GO

INSERT INTO PIMS_DOCUMENT_STATUS_TYPE (DOCUMENT_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'NONE',    N'None'),
  (N'DRAFT',   N'Draft'),
  (N'APPROVD', N'Approved'),
  (N'SENT',    N'Sent'),
  (N'SIGND',   N'Signed'),
  (N'AMENDD',  N'Amended'),
  (N'FINAL',   N'Final'),
  (N'RGSTRD',  N'Registered'),
  (N'UNREGD',  N'Unregistered'),
  (N'CNCLD',   N'Cancelled');
GO
