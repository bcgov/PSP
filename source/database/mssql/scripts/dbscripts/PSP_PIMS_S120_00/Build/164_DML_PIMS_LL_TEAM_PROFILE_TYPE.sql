-- -------------------------------------------------------------------------------------------
-- Populate the PIMS_LL_TEAM_PROFILE_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2026-Apr-01  PSP-11371  Designate team key contacts on file.  Added KEYCNTCT.
-- -------------------------------------------------------------------------------------------

DELETE FROM PIMS_LL_TEAM_PROFILE_TYPE
GO

INSERT INTO PIMS_LL_TEAM_PROFILE_TYPE (LL_TEAM_PROFILE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'KEYCNTCT',    N'Key contact',             0),
  (N'MOTTCONTACT', N'MoTT contact',            1),
  (N'MOTTLAWYER',  N'MoTT solicitor',          2),
  (N'PROPANALYST', N'Property analyst',        3),
  (N'PROPCOORD',   N'Property coordinator',    4),
  (N'PROPADMIN',   N'Property administrator',  5),
  (N'LANDPRJMGR',  N'Land project manager',    6),
  (N'LANDOPSMGR',  N'Land operations manager', 7);
GO
