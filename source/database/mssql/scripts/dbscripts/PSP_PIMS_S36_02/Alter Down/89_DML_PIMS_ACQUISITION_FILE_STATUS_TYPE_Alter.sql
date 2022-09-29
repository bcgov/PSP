/* -----------------------------------------------------------------------------
Insert new data into the PIMS_ACQUISITION_FILE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'DRAFT'

SELECT ACQUISITION_FILE_STATUS_TYPE_CODE
FROM   PIMS_ACQUISITION_FILE_STATUS_TYPE
WHERE  ACQUISITION_FILE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE 
  FROM   PIMS_ACQUISITION_FILE_STATUS_TYPE
  WHERE  ACQUISITION_FILE_STATUS_TYPE_CODE = @CurrCd;
  END
  

SET @CurrCd = N'HOLD'

SELECT ACQUISITION_FILE_STATUS_TYPE_CODE
FROM   PIMS_ACQUISITION_FILE_STATUS_TYPE
WHERE  ACQUISITION_FILE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE 
  FROM   PIMS_ACQUISITION_FILE_STATUS_TYPE
  WHERE  ACQUISITION_FILE_STATUS_TYPE_CODE = @CurrCd;
  END

COMMIT TRANSACTION;
