/* -----------------------------------------------------------------------------
Populate the PIMS_DSP_PHYS_FILE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Nov-21  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DSP_PHYS_FILE_STATUS_TYPE
GO

INSERT INTO PIMS_DSP_PHYS_FILE_STATUS_TYPE (DSP_PHYS_FILE_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACTIVE',  N'Active'),
  (N'ARCHIVE', N'Archive - Offsite'),
  (N'PENDING', N'Pending Litigation');
GO
