/* -----------------------------------------------------------------------------
Alter/add data to the PIMS_LEASE_PAYMENT_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

-- Enable the existing code values
UPDATE PIMS_LEASE_PAYMENT_STATUS_TYPE
SET    IS_DISABLED                = CONVERT([bit],(0))
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_PAYMENT_STATUS_TYPE_CODE IN ('PARTIAL', 'OVERPAID');

-- No rows changed so insert the required rows.
IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_LEASE_PAYMENT_STATUS_TYPE (LEASE_PAYMENT_STATUS_TYPE_CODE, DESCRIPTION)
  VALUES
    (N'PARTIAL',  N'Partial'),
    (N'OVERPAID', N'Overpayment');
  END

COMMIT TRANSACTION;
