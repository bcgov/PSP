/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_PMT_FREQ_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_PMT_FREQ_TYPE
GO

INSERT INTO PIMS_LEASE_PMT_FREQ_TYPE (LEASE_PMT_FREQ_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ANNUAL',  N'Annually'),
  (N'SEMIANN', N'Semi Annually'),
  (N'QUARTER', N'Quarterly'),
  (N'BIMONTH', N'Bi-Monthly'),
  (N'MONTHLY', N'Monthly'),
  (N'BIWEEK',  N'Bi-Weekly'),
  (N'WEEKLY',  N'Weekly'),
  (N'DAILY',   N'Daily'),
  (N'PREPAID', N'Prepaid / One Time'),
  (N'NOMINAL', N'Nominal ($1)');