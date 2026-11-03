/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_ACQ_CHKLST_SECTION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQ_CHKLST_SECTION_TYPE
GO

INSERT INTO PIMS_ACQ_CHKLST_SECTION_TYPE (ACQ_CHKLST_SECTION_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'FILEINIT',  N'File Initiation',           1),
  (N'FILEMGMT',  N'Active File Management',    2),
  (N'CROWNLND',  N'Crown Land',                3),
  (N'SCTN3AGR',  N'Section 3 - Agreement',     4),
  (N'SCTN6XPRP', N'Section 6 - Expropriation', 5),
  (N'ACQCOMPAC', N'Acquisition Completion',    6);
GO
