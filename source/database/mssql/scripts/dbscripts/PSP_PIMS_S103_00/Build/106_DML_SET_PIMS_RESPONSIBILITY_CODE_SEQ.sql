/* -----------------------------------------------------------------------------
Set the PIMS_RESPONSIBILITY_CODE_SEQ sequence to +1 over the current maximum 
CODE value.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Nov-08  Initial version
----------------------------------------------------------------------------- */

DECLARE @StartVlu bigint;
DECLARE @Qry nvarchar(max);

SET @StartVlu = (SELECT MAX(CODE) + 1 FROM PIMS_RESPONSIBILITY_CODE)
SET @Qry      = 'ALTER SEQUENCE PIMS_RESPONSIBILITY_CODE_SEQ RESTART WITH ' + CAST(@StartVlu AS NVARCHAR(20)) + ';'

EXEC SP_EXECUTESQL @Qry;

GO