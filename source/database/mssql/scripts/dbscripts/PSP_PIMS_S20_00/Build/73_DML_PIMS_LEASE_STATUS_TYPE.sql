/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_STATUS_TYPE
GO

INSERT INTO PIMS_LEASE_STATUS_TYPE (LEASE_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACTIVE',     N'Active'),
  (N'TERMINATED', N'Terminated'),
  (N'DRAFT',      N'Draft'),
  (N'DISCARD',    N'Discarded'),
  (N'INACTIVE',   N'Inactive');
