-- -------------------------------------------------------------------------------------------
-- Populate the PIMS_LL_TEAM_PROFILE_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2026-Apr-01  PSP-11371  Designate team key contacts on file.  Added KEYCNTCT.
-- Doug Filteau  2026-Apr-08  PSP-11395  Rename and disable "Key contact" to "PIMS key contact".
-- -------------------------------------------------------------------------------------------

DELETE FROM PIMS_LL_TEAM_PROFILE_TYPE
GO

INSERT INTO PIMS_LL_TEAM_PROFILE_TYPE (LL_TEAM_PROFILE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER, IS_DISABLED)
VALUES
  (N'KEYCNTCT',    N'PIMS key contact',        0, 1),
  (N'MOTTCONTACT', N'MoTT contact',            1, 0),
  (N'MOTTLAWYER',  N'MoTT solicitor',          2, 0),
  (N'PROPANALYST', N'Property analyst',        3, 0),
  (N'PROPCOORD',   N'Property coordinator',    4, 0),
  (N'PROPADMIN',   N'Property administrator',  5, 0),
  (N'LANDPRJMGR',  N'Land project manager',    6, 0),
  (N'LANDOPSMGR',  N'Land operations manager', 7, 0);
GO
