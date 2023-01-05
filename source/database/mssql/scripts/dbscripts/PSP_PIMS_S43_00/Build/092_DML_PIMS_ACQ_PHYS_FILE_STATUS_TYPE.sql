/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ACQ_PHYS_FILE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQ_PHYS_FILE_STATUS_TYPE
GO

INSERT INTO PIMS_ACQ_PHYS_FILE_STATUS_TYPE (ACQ_PHYS_FILE_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACTIVE',  N'Active'),
  (N'PENDING', N'Pending Litigation'),
  (N'ARCHIVE', N'Archive - Offsite');
GO
