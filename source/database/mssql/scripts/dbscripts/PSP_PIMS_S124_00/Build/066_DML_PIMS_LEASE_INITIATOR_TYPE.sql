/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_INITIATOR_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_INITIATOR_TYPE
GO

INSERT INTO PIMS_LEASE_INITIATOR_TYPE (LEASE_INITIATOR_TYPE_CODE, DESCRIPTION)
VALUES
  (N'PROJECT', N'Project'),
  (N'REGION',  N'Region'),
  (N'HQ',      N'Headquarters');