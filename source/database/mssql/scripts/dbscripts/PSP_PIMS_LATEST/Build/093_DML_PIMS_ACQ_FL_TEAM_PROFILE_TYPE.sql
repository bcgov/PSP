-- -------------------------------------------------------------------------------------------
-- Delete all data from the PIMS_ACQ_FL_TEAM_PROFILE_TYPE table and repopulate.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2021-Aug-24  N/A        Initial version
-- Doug Filteau  2026-Apr-01  PSP-11371  Designate team key contacts on file.  Added KEYCNTCT.
-- Doug Filteau  2026-Apr-08  PSP-11395  Rename and disable "Key contact" to "PIMS key contact".
-- -------------------------------------------------------------------------------------------

DELETE FROM PIMS_ACQ_FL_TEAM_PROFILE_TYPE
GO

INSERT INTO PIMS_ACQ_FL_TEAM_PROFILE_TYPE (ACQ_FL_TEAM_PROFILE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER, IS_DISABLED)
VALUES
  (N'KEYCNTCT',   N'PIMS key contact',     0, 1),
  (N'EXPRAGENT',  N'Expropriation agent',  1, 0),
  (N'MOTILAWYER', N'MoTT Solicitor',       2, 0),
  (N'NEGOTAGENT', N'Negotiation agent',    3, 0),
  (N'PROPAGENT',  N'Property agent',       4, 0),
  (N'PROPANLYS',  N'Property analyst',     5, 0),
  (N'PROPCOORD',  N'Property coordinator', 6, 0);
GO
