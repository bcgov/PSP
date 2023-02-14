/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_COST_TYPE_CODE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'WBO'

SELECT CODE
FROM   PIMS_COST_TYPE_CODE
WHERE  CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_COST_TYPE_CODE (CODE, DESCRIPTION, DISPLAY_ORDER, EFFECTIVE_DATE)
  VALUES 
    (N'WBO', N'WBO', 1000, CONVERT(DATETIME, '1990.01.01', 102));
  END

COMMIT TRANSACTION;
