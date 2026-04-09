-- -------------------------------------------------------------------------------------------
-- Delete all data from the PIMS_LEASE_STAKEHOLDER_TYPE table and repopulate.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2021-Aug-24  N/A        Initial version.
-- Doug Filteau  2025-Oct-15  N/A        Added OWNSOL.
-- Doug Filteau  2026-Jan-08  PSP-11108  Added OTHRCV and OTHPAY.
-- Doug Filteau  2026-Feb-27  PSP-11279  Amended to repair UNK and added OTHRCV.
-- -------------------------------------------------------------------------------------------

DELETE FROM PIMS_LEASE_STAKEHOLDER_TYPE
GO

INSERT INTO PIMS_LEASE_STAKEHOLDER_TYPE (LEASE_STAKEHOLDER_TYPE_CODE, DESCRIPTION, IS_PAYABLE_RELATED, IS_DISABLED)
VALUES
  (N'TEN',    N'Tenant',               0, 0),
  (N'ASGN',   N'Assignee',             0, 0),
  (N'REP',    N'Representative',       0, 0),
  (N'PMGR',   N'Property manager',     0, 0),
  (N'UNK',    N'Unknown',              0, 1),
  (N'OTHRCV', N'Other',                0, 0),
  (N'OTHPAY', N'Other',                1, 0),
  (N'OWNER',  N'Owner',                1, 0),
  (N'OWNREP', N'Owner Representative', 1, 0),
  (N'OWNSOL', N'Owner Solicitor',      1, 0);
GO

-- -------------------------------------------------------------------------------------------
-- Update the display order for the non-Other codes by DESCRIPTION
-- -------------------------------------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = biz.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_LEASE_STAKEHOLDER_TYPE biz JOIN
       (SELECT LEASE_STAKEHOLDER_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_LEASE_STAKEHOLDER_TYPE
        WHERE  DESCRIPTION <> N'Other') seq ON seq.LEASE_STAKEHOLDER_TYPE_CODE = biz.LEASE_STAKEHOLDER_TYPE_CODE
GO

-- -------------------------------------------------------------------------------------------
-- Update the display order for the Other codes
-- -------------------------------------------------------------------------------------------
UPDATE PIMS_LEASE_STAKEHOLDER_TYPE
SET    DISPLAY_ORDER              = 99
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  DESCRIPTION = N'Other'
GO
