/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_PAY_RVBL_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_PAY_RVBL_TYPE
GO

INSERT INTO PIMS_LEASE_PAY_RVBL_TYPE (LEASE_PAY_RVBL_TYPE_CODE, DESCRIPTION)
VALUES
  (N'RCVBL', N'Receivable'),
  (N'PYBLMOTI', N'Payable (MOTI as tenant)'),
  (N'PYBLBCTFA', N'Payable (BCTFA as tenant)');