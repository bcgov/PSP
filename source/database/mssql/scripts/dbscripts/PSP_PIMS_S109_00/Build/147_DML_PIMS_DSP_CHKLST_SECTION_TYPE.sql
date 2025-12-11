/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_DSP_CHKLST_SECTION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Dec-04  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DSP_CHKLST_SECTION_TYPE
GO

INSERT INTO PIMS_DSP_CHKLST_SECTION_TYPE (DSP_CHKLST_SECTION_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'FILEINIT', N'File Initiation',             1),
  (N'DISPPREP', N'Disposition Preparation',     2),
  (N'REFRCONS', N'Referrals and Consultations', 3),
  (N'DIRCTSAL', N'Direct Sale or Road Closure', 4),
  (N'SALEINFO', N'Sale Information',            5);
GO
