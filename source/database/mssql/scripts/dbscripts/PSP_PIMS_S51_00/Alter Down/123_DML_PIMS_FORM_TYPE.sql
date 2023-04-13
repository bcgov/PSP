/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_FORM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

-- Remove the "LETTER" type

DECLARE @FormType NVARCHAR(20)
SET     @FormType = N'LETTER'

SELECT FORM_TYPE_CODE
FROM   PIMS_FORM_TYPE
WHERE  FORM_TYPE_CODE = @FormType;

IF @@ROWCOUNT = 1
  BEGIN
    DELETE FROM PIMS_FORM_TYPE
    WHERE FORM_TYPE_CODE = @FormType
  END

