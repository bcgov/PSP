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

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_ACQUISITION_FILE_STATUS_TYPE (ACQUISITION_FILE_STATUS_TYPE_CODE, DESCRIPTION)
  VALUES 
    (N'DRAFT', N'Draft');
  END


SET @CurrCd = N'HOLD'

SELECT ACQUISITION_FILE_STATUS_TYPE_CODE
FROM   PIMS_ACQUISITION_FILE_STATUS_TYPE
WHERE  ACQUISITION_FILE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_ACQUISITION_FILE_STATUS_TYPE (ACQUISITION_FILE_STATUS_TYPE_CODE, DESCRIPTION)
  VALUES 
    (N'HOLD', N'Hold');
  END

COMMIT TRANSACTION;
