-- -------------------------------------------------------------------------------------------
-- Populate the PIMS_DSP_FL_TEAM_PROFILE_TYPE table and repopulate.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2021-Aug-24  N/A        Initial version
-- Doug Filteau  2026-Apr-01  PSP-11371  Designate team key contacts on file.  Added KEYCNTCT.
-- -------------------------------------------------------------------------------------------

DELETE FROM PIMS_DSP_FL_TEAM_PROFILE_TYPE
GO

INSERT INTO PIMS_DSP_FL_TEAM_PROFILE_TYPE (DSP_FL_TEAM_PROFILE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'KEYCNTCT',   N'Key contact',          0),
  (N'LISTAGENT',  N'Listing agent',        1),
  (N'MOTILEAD',   N'MoTT lead',            2),
  (N'MOTILAWYER', N'MoTT solicitor',       3),
  (N'NEGOTAGENT', N'Negotiation agent',    4),
  (N'PROPCOORD',  N'Property coordinator', 5),
  (N'PROPAGENT',  N'Property agent',       6),
  (N'PROPANLYS',  N'Property analyst',     7);
GO
