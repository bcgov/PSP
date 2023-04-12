/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_FORM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

-- Enable the "LETTER" type

DECLARE @FormType NVARCHAR(20)
SET     @FormType = N'LETTER'

SELECT FORM_TYPE_CODE
FROM   PIMS_FORM_TYPE
WHERE  FORM_TYPE_CODE = @FormType;

IF @@ROWCOUNT = 0
  BEGIN
    INSERT INTO PIMS_FORM_TYPE (FORM_TYPE_CODE, DESCRIPTION)
    VALUES
      (N'LETTER',  N'General Letter')
  END

