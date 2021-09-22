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
  (N'NOMINAL', N'Nominal'),
  (N'MONTHLY', N'Monthly'),
  (N'PREPAID', N'Prepaid'),
  (N'ANNUAL', N'Annual');