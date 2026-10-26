/* -----------------------------------------------------------------------------
Populate the PIMS_DISPOSITION_INITIATING_DOC_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Nov-21  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DISPOSITION_INITIATING_DOC_TYPE
GO

INSERT INTO PIMS_DISPOSITION_INITIATING_DOC_TYPE (DISPOSITION_INITIATING_DOC_TYPE_CODE, DESCRIPTION)
VALUES
  (N'NOTNEEDED', N'Not Needed'),
  (N'OTHER',     N'Other'),
  (N'H0222',     N'H0222'),
  (N'SURPLUS',   N'Surplus Declaration');
GO
