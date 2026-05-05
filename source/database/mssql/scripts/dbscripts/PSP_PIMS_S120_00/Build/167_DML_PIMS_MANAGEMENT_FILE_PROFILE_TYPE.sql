-- -------------------------------------------------------------------------------------------
-- Populate the PIMS_MANAGEMENT_FILE_PROFILE_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2025-Apr-11  N/A        Initial version
-- Doug Filteau  2026-Apr-01  PSP-11371  Designate team key contacts on file.  Added KEYCNTCT.
-- Doug Filteau  2026-Apr-08  PSP-11395  Rename and disable "Key contact" to "PIMS key contact".
-- -------------------------------------------------------------------------------------------

DELETE FROM PIMS_MANAGEMENT_FILE_PROFILE_TYPE
GO

INSERT INTO PIMS_MANAGEMENT_FILE_PROFILE_TYPE (MANAGEMENT_FILE_PROFILE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER, IS_DISABLED)
VALUES
  (N'KEYCNTCT', N'PIMS key contact', 0, 1),
  (N'CONTRACT', N'Contractor',       1, 0),
  (N'MINSTAFF', N'MoTT staff',       2, 0);
GO
