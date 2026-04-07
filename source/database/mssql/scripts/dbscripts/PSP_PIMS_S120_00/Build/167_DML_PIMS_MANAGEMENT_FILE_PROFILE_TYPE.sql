-- -------------------------------------------------------------------------------------------
-- Populate the PIMS_MANAGEMENT_FILE_PROFILE_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2025-Apr-11  N/A        Initial version
-- Doug Filteau  2026-Apr-01  PSP-11371  Designate team key contacts on file.  Added KEYCNTCT.
-- -------------------------------------------------------------------------------------------

DELETE FROM PIMS_MANAGEMENT_FILE_PROFILE_TYPE
GO

INSERT INTO PIMS_MANAGEMENT_FILE_PROFILE_TYPE (MANAGEMENT_FILE_PROFILE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'KEYCNTCT', N'Key contact', 0),
  (N'CONTRACT', N'Contractor',  1),
  (N'MINSTAFF', N'MoTT staff',  2);
GO
