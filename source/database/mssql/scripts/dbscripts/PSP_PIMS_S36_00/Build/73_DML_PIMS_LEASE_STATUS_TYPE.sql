/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_STATUS_TYPE
GO

INSERT INTO PIMS_LEASE_STATUS_TYPE (LEASE_STATUS_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'ACTIVE',     N'Active',     CONVERT([bit],(0))),
  (N'EXPIRED',    N'Expired',    CONVERT([bit],(1))),
  (N'TERMINATED', N'Terminated', CONVERT([bit],(0))),
  (N'DRAFT',      N'Draft',      CONVERT([bit],(0))),
  (N'DISCARD',    N'Discarded',  CONVERT([bit],(0))),
  (N'INACTIVE',   N'Inactive',   CONVERT([bit],(0)));
