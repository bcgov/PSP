/* -----------------------------------------------------------------------------
Update the PIMS_PPH_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Apr-12  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'107EXP';

SELECT PROPERTY_ROAD_TYPE_CODE
FROM   PIMS_PROPERTY_ROAD_TYPE
WHERE  PROPERTY_ROAD_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_PROPERTY_ROAD_TYPE 
  SET    DESCRIPTION                = N'107 Explanatory Plan'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PROPERTY_ROAD_TYPE_CODE = @CurrCd;
  END

COMMIT TRANSACTION;
