/* -----------------------------------------------------------------------------
Insert new data into the PIMS_ACTIVITY_INSTANCE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'COMPLETE'

SELECT ACTIVITY_INSTANCE_STATUS_TYPE_CODE
FROM   PIMS_ACTIVITY_INSTANCE_STATUS_TYPE
WHERE  ACTIVITY_INSTANCE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_ACTIVITY_INSTANCE_STATUS_TYPE
  SET    DESCRIPTION                = N'Complete'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  ACTIVITY_INSTANCE_STATUS_TYPE_CODE = @CurrCd;
  END

COMMIT TRANSACTION;
