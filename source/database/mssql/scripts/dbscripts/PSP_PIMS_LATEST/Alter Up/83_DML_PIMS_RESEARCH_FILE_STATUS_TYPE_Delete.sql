/* -----------------------------------------------------------------------------
Delete data from the PIMS_RESEARCH_FILE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'ANY'

SELECT RESEARCH_FILE_STATUS_TYPE_CODE
FROM   PIMS_RESEARCH_FILE_STATUS_TYPE
WHERE  RESEARCH_FILE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  BEGIN
  DELETE 
  FROM   PIMS_RESEARCH_FILE_STATUS_TYPE
  WHERE  RESEARCH_FILE_STATUS_TYPE_CODE = @CurrCd;
  END

COMMIT TRANSACTION;
