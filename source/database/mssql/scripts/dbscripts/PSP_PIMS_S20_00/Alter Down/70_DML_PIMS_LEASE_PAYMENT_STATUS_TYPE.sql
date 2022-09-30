/* -----------------------------------------------------------------------------
Disable data in the PIMS_LEASE_PAYMENT_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

-- Disable the existing code values
UPDATE PIMS_LEASE_PAYMENT_STATUS_TYPE
SET    IS_DISABLED                = CONVERT([bit],(1))
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_PAYMENT_STATUS_TYPE_CODE IN ('PARTIAL', 'OVERPAID');

COMMIT TRANSACTION;
