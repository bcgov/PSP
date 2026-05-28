/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_STAKEHOLDER_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version.
Doug Filteau  2025-Oct-15  Added OWNSOL.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_STAKEHOLDER_TYPE
GO

INSERT INTO PIMS_LEASE_STAKEHOLDER_TYPE (LEASE_STAKEHOLDER_TYPE_CODE, DESCRIPTION, IS_PAYABLE_RELATED)
VALUES
  (N'TEN',    N'Tenant',               0),
  (N'ASGN',   N'Assignee',             0),
  (N'REP',    N'Representative',       0),
  (N'PMGR',   N'Property manager',     0),
  (N'UNK',    N'Unknown',              0),
  (N'OWNER',  N'Owner',                1),
  (N'OWNREP', N'Owner Representative', 1),
  (N'OWNSOL', N'Owner Solicitor',      1);
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_LEASE_STAKEHOLDER_TYPE biz JOIN
       (SELECT LEASE_STAKEHOLDER_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_LEASE_STAKEHOLDER_TYPE) seq  ON seq.LEASE_STAKEHOLDER_TYPE_CODE = biz.LEASE_STAKEHOLDER_TYPE_CODE
GO
