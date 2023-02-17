/* -----------------------------------------------------------------------------
Delete the missing code values in the PIMS_COST_TYPE_CODE table.
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

IF @@ROWCOUNT = 1
  BEGIN
  DELETE 
  FROM   PIMS_COST_TYPE_CODE 
  WHERE  CODE = @CurrCd;
  END

COMMIT TRANSACTION;
