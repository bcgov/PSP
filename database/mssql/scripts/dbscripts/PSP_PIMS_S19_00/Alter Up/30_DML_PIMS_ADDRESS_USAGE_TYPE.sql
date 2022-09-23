/* -----------------------------------------------------------------------------
Alter the usage type code description for the RESIDNT code value.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Jan-17  Initial version
----------------------------------------------------------------------------- */

-- Set all current address usage type codes to disabled.
UPDATE PIMS_ADDRESS_USAGE_TYPE
SET    DESCRIPTION = N'Property address'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  ADDRESS_USAGE_TYPE_CODE = 'RESIDNT';
GO
