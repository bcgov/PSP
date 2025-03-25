/* -----------------------------------------------------------------------------
Populate the PIMS_LL_TEAM_PROFILE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Feb-07  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LL_TEAM_PROFILE_TYPE
GO

INSERT INTO PIMS_LL_TEAM_PROFILE_TYPE (LL_TEAM_PROFILE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'EXPROPAGENT', N'Expropriation agent'),
  (N'MOTILAWYER',  N'MoTI Solicitor'),
  (N'NEGOTAGENT',  N'Negotiation agent'),
  (N'PROPAGENT',   N'Property agent'),
  (N'PROPANALYST', N'Property analyst'),
  (N'PROPCOORD',   N'Property coordinator');
GO
