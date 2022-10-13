/* -----------------------------------------------------------------------------
Delete data from the PIMS_DATA_SOURCE_TYPE table.
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

IF @@ROWCOUNT = 1
  BEGIN
  DELETE 
  FROM   PIMS_DATA_SOURCE_TYPE
  WHERE  DATA_SOURCE_TYPE_CODE = @CurrCd;
  END

COMMIT TRANSACTION;
