/* -----------------------------------------------------------------------------
Disable all existing data on the PIMS_ADDRESS_USAGE_TYPE table and insert new 
usage type codes.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-09  Initial version
----------------------------------------------------------------------------- */

-- Set all current address usage type codes to disabled.
UPDATE PIMS_ADDRESS_USAGE_TYPE
SET    IS_DISABLED = CONVERT([bit],(1))
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  ADDRESS_USAGE_TYPE_CODE <> 'UNKNOWN';
GO

-- Insert the new address usage type codes into the table.
INSERT INTO PIMS_ADDRESS_USAGE_TYPE (ADDRESS_USAGE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'MAILADDR',   N'Mailing address',               CONVERT([bit],(1))),
  (N'INSURE',     N'Proof of insurance address',    CONVERT([bit],(1))),
  (N'RENTAL',     N'Rental payment address',        CONVERT([bit],(1))),
  (N'PROPNOTIFY', N'Property notification address', CONVERT([bit],(1))),
  (N'PHYSADDR',   N'Physical address',              CONVERT([bit],(1))),
  (N'MAILING',    N'Mailing address',               CONVERT([bit],(0))),
  (N'BILLING',    N'Billing address',               CONVERT([bit],(0))),
  (N'RESIDNT',    N'Residential address',           CONVERT([bit],(0))),
  (N'ETLUNKN ',   N'ETL unknown address',           CONVERT([bit],(0))),
  (N'UNKNOWN',    N'Unknown address type',          CONVERT([bit],(0)));