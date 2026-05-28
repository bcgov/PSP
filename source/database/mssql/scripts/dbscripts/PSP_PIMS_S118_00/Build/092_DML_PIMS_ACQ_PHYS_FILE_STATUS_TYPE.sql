/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ACQ_PHYS_FILE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version.
Doug Filteau  2025-Feb-13  Added UNKNOWN and set display order.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQ_PHYS_FILE_STATUS_TYPE
GO

INSERT INTO PIMS_ACQ_PHYS_FILE_STATUS_TYPE (ACQ_PHYS_FILE_STATUS_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'ACTIVE',  N'Active',              1),
  (N'ARCHIVE', N'Archive - Offsite',   2),
  (N'PENDING', N'Pending Litigation',  3),
  (N'UNKNOWN', N'Unknown',            99);
GO
