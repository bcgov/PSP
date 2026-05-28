/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_PAYMENT_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_PAYMENT_STATUS_TYPE
GO

INSERT INTO PIMS_LEASE_PAYMENT_STATUS_TYPE (LEASE_PAYMENT_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'PAID',     N'Paid'),
  (N'UNPAID',   N'Unpaid'),
  (N'PARTIAL',  N'Partial'),
  (N'OVERPAID', N'Overpayment');