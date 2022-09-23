/* -----------------------------------------------------------------------------
Delete all data from the PIMS_INSURANCE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

UPDATE PIMS_INSURANCE_TYPE
SET    DISPLAY_ORDER = 2
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  INSURANCE_TYPE_CODE = 'GENERAL';

UPDATE PIMS_INSURANCE_TYPE
SET    DISPLAY_ORDER = 4
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  INSURANCE_TYPE_CODE = 'VEHICLE';

UPDATE PIMS_INSURANCE_TYPE
SET    DISPLAY_ORDER = 1
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  INSURANCE_TYPE_CODE = 'AIRCRAFT';

UPDATE PIMS_INSURANCE_TYPE
SET    DISPLAY_ORDER = 3
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  INSURANCE_TYPE_CODE = 'MARINE';

UPDATE PIMS_INSURANCE_TYPE
SET    DISPLAY_ORDER = 99
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  INSURANCE_TYPE_CODE = 'OTHER';
