/* -----------------------------------------------------------------------------
Populate the PIMS_AGREEMENT_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Oct-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_AGREEMENT_STATUS_TYPE
GO

INSERT INTO PIMS_AGREEMENT_STATUS_TYPE (AGREEMENT_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'DRAFT',     N'Draft'),
  (N'FINAL',     N'Final'),
  (N'CANCELLED', N'Cancelled');
GO
