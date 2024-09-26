/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_STAKEHOLDER_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
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
  (N'OWNREP', N'Owner Representative', 1);
GO
