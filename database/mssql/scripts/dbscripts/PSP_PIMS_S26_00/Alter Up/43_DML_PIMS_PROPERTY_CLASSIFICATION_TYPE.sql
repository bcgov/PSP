/* -----------------------------------------------------------------------------
Insert new data into the PIMS_PROPERTY_CLASSIFICATION_TYPE table.
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

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_PROPERTY_CLASSIFICATION_TYPE (PROPERTY_CLASSIFICATION_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
  VALUES 
    (N'UNKNOWN', N'Unknown', 8);
  END

COMMIT TRANSACTION;
