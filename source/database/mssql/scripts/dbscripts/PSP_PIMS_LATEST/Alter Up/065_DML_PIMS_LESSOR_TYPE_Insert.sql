/* -----------------------------------------------------------------------------
Insert data into the PIMS_LESSOR_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Nov-22  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'UNK'

SELECT LESSOR_TYPE_CODE
FROM   PIMS_LESSOR_TYPE
WHERE  LESSOR_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_LESSOR_TYPE (LESSOR_TYPE_CODE, DESCRIPTION)
  VALUES 
    (N'UNK', N'Unknown');
  END

COMMIT TRANSACTION;
