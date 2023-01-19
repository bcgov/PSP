/* -----------------------------------------------------------------------------
Delete all data from the PIMS_TENANT_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_TENANT_TYPE
GO

INSERT INTO PIMS_TENANT_TYPE (TENANT_TYPE_CODE, DESCRIPTION)
VALUES
  (N'TEN',  N'Tenant'),
  (N'REP',  N'Representative'),
  (N'PMGR', N'Property manager'),
  (N'UNK',  N'Unknown');
  