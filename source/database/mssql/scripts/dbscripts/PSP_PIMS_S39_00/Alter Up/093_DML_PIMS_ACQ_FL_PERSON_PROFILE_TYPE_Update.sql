/* -----------------------------------------------------------------------------
Update data in the PIMS_ACQ_FL_PERSON_PROFILE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Oct-06  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'PROPCOORD'

SELECT ACQ_FL_PERSON_PROFILE_TYPE_CODE
FROM   PIMS_ACQ_FL_PERSON_PROFILE_TYPE
WHERE  ACQ_FL_PERSON_PROFILE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  UPDATE PIMS_ACQ_FL_PERSON_PROFILE_TYPE
  SET    DESCRIPTION = N'Property coordinator'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  ACQ_FL_PERSON_PROFILE_TYPE_CODE = @CurrCd;
  END

COMMIT TRANSACTION;