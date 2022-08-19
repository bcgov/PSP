/* -----------------------------------------------------------------------------
Insert new data into the PIMS_RESEARCH_FILE_STATUS_TYPE table.
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

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_RESEARCH_FILE_STATUS_TYPE (RESEARCH_FILE_STATUS_TYPE_CODE, DESCRIPTION)
  VALUES 
    (N'ANY', N'Any status');
  END

COMMIT TRANSACTION;
