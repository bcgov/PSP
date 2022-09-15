/* -----------------------------------------------------------------------------
Insert new data into the PIMS_ACQ_FL_PERSON_PROFILE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'PROPANLYS'

SELECT ACQ_FL_PERSON_PROFILE_TYPE_CODE
FROM   PIMS_ACQ_FL_PERSON_PROFILE_TYPE
WHERE  ACQ_FL_PERSON_PROFILE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE 
  FROM   PIMS_ACQ_FL_PERSON_PROFILE_TYPE
  WHERE  ACQ_FL_PERSON_PROFILE_TYPE_CODE = @CurrCd;
  END

COMMIT TRANSACTION;
