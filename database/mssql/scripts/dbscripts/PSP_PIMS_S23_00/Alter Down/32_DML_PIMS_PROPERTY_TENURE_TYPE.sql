/* -----------------------------------------------------------------------------
Delete some existing codes in the PIMS_PROPERTY_TENURE_TYPE table and re-enable 
the legacy codes.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Mar-08  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

-- Enable the legacy code values
UPDATE PIMS_PROPERTY_TENURE_TYPE
SET    IS_DISABLED                = CONVERT([bit],(0))
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  PROPERTY_TENURE_TYPE_CODE IN (N'PL', N'TM', N'CL', N'TT', N'RW', N'CR');
GO

-- Delete the new code values
DELETE
FROM   PIMS_PROPERTY_TENURE_TYPE
WHERE  PROPERTY_TENURE_TYPE_CODE IN(N'HWYROAD',  N'ADJLAND',  N'CLOSEDRD', N'UNKNOWN');
GO

COMMIT TRANSACTION;
