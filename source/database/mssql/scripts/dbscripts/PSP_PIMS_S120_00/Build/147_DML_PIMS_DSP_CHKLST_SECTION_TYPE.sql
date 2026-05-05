-- -------------------------------------------------------------------------------------------
-- Populate the missing code values in the PIMS_DSP_CHKLST_SECTION_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2023-Dec-04  N/A        Initial version
-- Doug Filteau  2025-Dec-08  PSP-11005  Renamed DIRCTSAL desciption.
-- Doug Filteau  2026-Jan-23  PSP-11131  Renamed DIRCTSAL desciption (again).
-- -------------------------------------------------------------------------------------------

DELETE FROM PIMS_DSP_CHKLST_SECTION_TYPE
GO

INSERT INTO PIMS_DSP_CHKLST_SECTION_TYPE (DSP_CHKLST_SECTION_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'FILEINIT', N'File Initiation',                    1),
  (N'DISPPREP', N'Disposition Preparation',            2),
  (N'REFRCONS', N'Referrals and Consultations',        3),
  (N'DIRCTSAL', N'Direct Sale or Road Closure or SRW', 4),
  (N'SALEINFO', N'Sale Information',                   5);
GO
