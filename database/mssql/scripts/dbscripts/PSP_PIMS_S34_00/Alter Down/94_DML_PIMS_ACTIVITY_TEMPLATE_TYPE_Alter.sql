/* -----------------------------------------------------------------------------
Insert new data into the PIMS_ACTIVITY_TEMPLATE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'GENLTR'

SELECT ACTIVITY_TEMPLATE_TYPE_CODE
FROM   PIMS_ACTIVITY_TEMPLATE_TYPE
WHERE  ACTIVITY_TEMPLATE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE 
  FROM   PIMS_ACTIVITY_TEMPLATE_TYPE
  WHERE  ACTIVITY_TEMPLATE_TYPE_CODE = @CurrCd;
  END

COMMIT TRANSACTION;
