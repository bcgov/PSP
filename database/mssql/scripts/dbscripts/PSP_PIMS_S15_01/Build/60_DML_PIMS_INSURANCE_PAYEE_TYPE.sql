/* -----------------------------------------------------------------------------
Delete all data from the PIMS_INSURANCE_PAYEE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_INSURANCE_PAYEE_TYPE
GO

INSERT INTO PIMS_INSURANCE_PAYEE_TYPE (INSURANCE_PAYEE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'SELF',     N'Self-Insured'),
  (N'REPLCOST', N'Replacement Cost Value'),
  (N'SELFXCSS', N'Self + Excess'),
  (N'OTHER',    N'Other');