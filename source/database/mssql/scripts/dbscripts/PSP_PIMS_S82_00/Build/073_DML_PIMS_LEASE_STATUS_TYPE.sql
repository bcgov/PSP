/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
Doug Filteau  2024-May-02  Added "Archived", renamed "Discarded" to "Cancelled" 
                           and "Inactive" to "Hold"
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_STATUS_TYPE
GO

INSERT INTO PIMS_LEASE_STATUS_TYPE (LEASE_STATUS_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'ACTIVE',     N'Active',     CONVERT([bit],(0))),
  (N'EXPIRED',    N'Expired',    CONVERT([bit],(1))),
  (N'TERMINATED', N'Terminated', CONVERT([bit],(0))),
  (N'DRAFT',      N'Draft',      CONVERT([bit],(0))),
  (N'DISCARD',    N'Cancelled',  CONVERT([bit],(0))),
  (N'INACTIVE',   N'Hold',       CONVERT([bit],(0))),
  (N'DUPLICATE',  N'Duplicate',  CONVERT([bit],(0))),
  (N'ARCHIVED',   N'Archived',   CONVERT([bit],(0)));
