/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_PAYMENT_METHOD_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_PAYMENT_METHOD_TYPE
GO

INSERT INTO PIMS_LEASE_PAYMENT_METHOD_TYPE (LEASE_PAYMENT_METHOD_TYPE_CODE, DESCRIPTION)
VALUES
  (N'CASH', N'Cash'),
  (N'CHEQ', N'Cheque'),
  (N'POST', N'Post-dated cheque'),
  (N'CRDR', N'Credit / Debit'),
  (N'EFT',  N'EFT'),
  (N'OTHR', N'Other'),
  (N'H120', N'H120');