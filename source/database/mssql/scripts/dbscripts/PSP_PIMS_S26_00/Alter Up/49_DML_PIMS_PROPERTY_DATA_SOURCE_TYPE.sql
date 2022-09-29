/* -----------------------------------------------------------------------------
Insert data into the PIMS_DATA_SOURCE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Apr-12  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'PMBC'

SELECT DATA_SOURCE_TYPE_CODE
FROM   PIMS_DATA_SOURCE_TYPE
WHERE  DATA_SOURCE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_DATA_SOURCE_TYPE (DATA_SOURCE_TYPE_CODE, DESCRIPTION) 
  VALUES
    (N'PMBC',  N'ParcelMap BC');
  END

COMMIT TRANSACTION;
