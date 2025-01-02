/* -----------------------------------------------------------------------------
Populate the PIMS_DSP_FL_TEAM_PROFILE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DSP_FL_TEAM_PROFILE_TYPE
GO

INSERT INTO PIMS_DSP_FL_TEAM_PROFILE_TYPE (DSP_FL_TEAM_PROFILE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'NEGOTAGENT', N'Negotiation agent'),
  (N'PROPCOORD',  N'Property coordinator'),
  (N'PROPAGENT',  N'Property agent'),
  (N'PROPANLYS',  N'Property analyst'),
  (N'MOTILAWYER', N'MoTI Solicitor'),
  (N'LISTAGENT',  N'Listing Agent'),
  (N'MOTILEAD',   N'MoTI Lead');
GO
