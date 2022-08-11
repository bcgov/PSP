/* -----------------------------------------------------------------------------
Delete all data from the PIMS_FILE_PERSON_PROFILE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_FILE_PERSON_PROFILE_TYPE
GO

INSERT INTO PIMS_FILE_PERSON_PROFILE_TYPE (FILE_PERSON_PROFILE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'NEGOTAGENT', N'Negotiation agent'),
  (N'PROPCOORD',  N'Property Coordinator'),
  (N'EXPRAGENT',  N'Expropriation agent'),
  (N'PROPAGENT',  N'Property agent');
GO
