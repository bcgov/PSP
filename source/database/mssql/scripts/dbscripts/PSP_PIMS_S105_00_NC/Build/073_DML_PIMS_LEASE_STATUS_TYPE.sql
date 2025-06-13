/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
Doug Filteau  2024-May-02  Added "Archived", renamed "Discarded" to "Cancelled" 
                           and "Inactive" to "Hold"
Doug Filteau  2025-Feb-04  Display order enforced
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_STATUS_TYPE
GO

INSERT INTO PIMS_LEASE_STATUS_TYPE (LEASE_STATUS_TYPE_CODE, DESCRIPTION, IS_DISABLED, DISPLAY_ORDER)
VALUES
  (N'ACTIVE',     N'Active',     CONVERT([bit],(0)), 1),
  (N'DRAFT',      N'Draft',      CONVERT([bit],(0)), 2),
  (N'DUPLICATE',  N'Duplicate',  CONVERT([bit],(0)), 3),
  (N'INACTIVE',   N'Hold',       CONVERT([bit],(0)), 4),
  (N'DISCARD',    N'Cancelled',  CONVERT([bit],(0)), 5),
  (N'TERMINATED', N'Terminated', CONVERT([bit],(0)), 6),
  (N'ARCHIVED',   N'Archived',   CONVERT([bit],(0)), 7),
  (N'EXPIRED',    N'Expired',    CONVERT([bit],(1)), 8);
GO
