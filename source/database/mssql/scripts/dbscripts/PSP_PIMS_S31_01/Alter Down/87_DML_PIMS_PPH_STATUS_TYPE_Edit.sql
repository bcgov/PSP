/* -----------------------------------------------------------------------------
Update the PIMS_PPH_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Apr-12  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'COMBO';

SELECT PPH_STATUS_TYPE_CODE
FROM   PIMS_PPH_STATUS_TYPE
WHERE  PPH_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_PPH_STATUS_TYPE 
  SET    DESCRIPTION                = N'Combination (PPH & Non-PPH)'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  PPH_STATUS_TYPE_CODE = @CurrCd;
  END

COMMIT TRANSACTION;
