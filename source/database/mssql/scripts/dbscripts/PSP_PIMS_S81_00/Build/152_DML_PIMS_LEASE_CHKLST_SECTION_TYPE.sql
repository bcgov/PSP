/* -----------------------------------------------------------------------------
Populate the PIMS_LEASE_CHKLST_SECTION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-May-17  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_CHKLST_SECTION_TYPE
GO

INSERT INTO PIMS_LEASE_CHKLST_SECTION_TYPE (LEASE_CHKLST_SECTION_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'FILEINIT',  N'File Initiation',          1),
  (N'REFERAPPR', N'Referrals and Approvals',  2),
  (N'AGREEPREP', N'Agreement Preparation',    3),
  (N'LLCOMPLTN', N'Lease/Licence Completion', 4);
GO
