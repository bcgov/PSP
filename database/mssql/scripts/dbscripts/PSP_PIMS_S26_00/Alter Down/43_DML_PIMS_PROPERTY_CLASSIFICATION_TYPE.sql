/* -----------------------------------------------------------------------------
Delete data from the PIMS_PROPERTY_CLASSIFICATION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Apr-12  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'UNKNOWN'

SELECT PROPERTY_CLASSIFICATION_TYPE_CODE
FROM   PIMS_PROPERTY_CLASSIFICATION_TYPE
WHERE  PROPERTY_CLASSIFICATION_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE 
  FROM   PIMS_PROPERTY_CLASSIFICATION_TYPE
  WHERE  PROPERTY_CLASSIFICATION_TYPE_CODE = @CurrCd;
  END

COMMIT TRANSACTION;
