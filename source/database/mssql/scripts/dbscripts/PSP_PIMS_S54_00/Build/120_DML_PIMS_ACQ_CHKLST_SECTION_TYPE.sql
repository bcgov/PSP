/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_ACQ_CHKLST_SECTION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQ_CHKLST_SECTION_TYPE
GO

INSERT INTO PIMS_ACQ_CHKLST_SECTION_TYPE (ACQ_CHKLST_SECTION_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER, EFFECTIVE_DATE)
VALUES
  (N'FILEINIT',  N'File Initiation',           1, getutcdate()),
  (N'FILEMGMT',  N'Active File Management',    2, getutcdate()),
  (N'SCTN3AGR',  N'Section 3 - Agreement',     3, getutcdate()),
  (N'SCTN6XPRP', N'Section 6 - Expropriation', 4, getutcdate()),
  (N'ACQCOMPAC', N'Acquisition Completion',    5, getutcdate());
GO
