/* -----------------------------------------------------------------------------
Amend all data from the PIMS_PROPERTY_IMPROVEMENT_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

UPDATE PIMS_PROPERTY_IMPROVEMENT_TYPE
SET    DISPLAY_ORDER = 2
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  PROPERTY_IMPROVEMENT_TYPE_CODE = 'RTA';

UPDATE PIMS_PROPERTY_IMPROVEMENT_TYPE
SET    DISPLAY_ORDER = 1
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  PROPERTY_IMPROVEMENT_TYPE_CODE = 'COMMBLDG';

UPDATE PIMS_PROPERTY_IMPROVEMENT_TYPE
SET    DISPLAY_ORDER = 99
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  PROPERTY_IMPROVEMENT_TYPE_CODE = 'OTHER';
