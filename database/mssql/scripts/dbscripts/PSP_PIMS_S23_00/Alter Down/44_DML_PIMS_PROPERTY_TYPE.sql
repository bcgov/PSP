/* -----------------------------------------------------------------------------
Delete some existing codes in the PIMS_PROPERTY_TYPE table and re-enable the 
legacy codes.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Mar-09  Initial version
----------------------------------------------------------------------------- */
  
BEGIN TRANSACTION;

-- Enable the existing legacy code values
UPDATE PIMS_PROPERTY_TYPE
SET    IS_DISABLED                =  CONVERT([bit],(0))
     , CONCURRENCY_CONTROL_NUMBER =  CONCURRENCY_CONTROL_NUMBER + 1
WHERE  PROPERTY_TYPE_CODE         IN (N'LAND', N'BUILD', N'SUBDIV');

-- Delete the new code values
DELETE 
FROM   PIMS_PROPERTY_TYPE 
WHERE  PROPERTY_TYPE_CODE IN (N'TITLED', N'HWYROAD', N'STRATACP', N'CROWNPSRVD', N'CROWNPUSRVD', N'CROWNFSRVD',  N'CROWNFUSRVD', N'RESERVE', N'PARKS', N'UNKNOWN');

COMMIT TRANSACTION;
