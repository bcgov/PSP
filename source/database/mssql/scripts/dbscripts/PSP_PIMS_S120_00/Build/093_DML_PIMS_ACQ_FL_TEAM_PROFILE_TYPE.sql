-- -------------------------------------------------------------------------------------------
-- Delete all data from the PIMS_ACQ_FL_TEAM_PROFILE_TYPE table and repopulate.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2021-Aug-24  N/A        Initial version
-- Doug Filteau  2026-Apr-01  PSP-11371  Designate team key contacts on file.  Added KEYCNTCT.
-- -------------------------------------------------------------------------------------------

DELETE FROM PIMS_ACQ_FL_TEAM_PROFILE_TYPE
GO

INSERT INTO PIMS_ACQ_FL_TEAM_PROFILE_TYPE (ACQ_FL_TEAM_PROFILE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'KEYCNTCT',   N'Key contact',          0),
  (N'EXPRAGENT',  N'Expropriation agent',  1),
  (N'MOTILAWYER', N'MoTT Solicitor',       2),
  (N'NEGOTAGENT', N'Negotiation agent',    3),
  (N'PROPAGENT',  N'Property agent',       4),
  (N'PROPANLYS',  N'Property analyst',     5),
  (N'PROPCOORD',  N'Property coordinator', 6);
GO
