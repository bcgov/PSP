/* -----------------------------------------------------------------------------
Populate the PIMS_LL_TEAM_PROFILE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Feb-07  Initial version
Doug Filteau  2025-Mar-06  Amended codes.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LL_TEAM_PROFILE_TYPE
GO

INSERT INTO PIMS_LL_TEAM_PROFILE_TYPE (LL_TEAM_PROFILE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'MOTTCONTACT', N'MoTT contact',            1),
  (N'MOTTLAWYER',  N'MoTT Solicitor',          2),
  (N'PROPANALYST', N'Property analyst',        3),
  (N'PROPCOORD',   N'Property coordinator',    4),
  (N'PROPADMIN',   N'Property administrator',  5),
  (N'LANDPRJMGR',  N'Land project manager',    6),
  (N'LANDOPSMGR',  N'Land operations manager', 7);
GO
