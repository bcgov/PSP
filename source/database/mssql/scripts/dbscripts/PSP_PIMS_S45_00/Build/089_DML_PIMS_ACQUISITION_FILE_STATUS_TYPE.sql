/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ACQUISITION_FILE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQUISITION_FILE_STATUS_TYPE
GO

INSERT INTO PIMS_ACQUISITION_FILE_STATUS_TYPE (ACQUISITION_FILE_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACTIVE', N'Active'),
  (N'CANCEL', N'Cancelled'),
  (N'CLOSED', N'Closed'),
  (N'ARCHIV', N'Archived'),
  (N'DRAFT',  N'Draft'),
  (N'HOLD',   N'Hold');
GO
