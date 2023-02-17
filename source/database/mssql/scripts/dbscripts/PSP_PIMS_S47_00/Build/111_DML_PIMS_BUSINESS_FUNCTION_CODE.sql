/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_BUSINESS_FUNCTION_CODE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_BUSINESS_FUNCTION_CODE
GO

INSERT INTO PIMS_BUSINESS_FUNCTION_CODE (CODE, DESCRIPTION, DISPLAY_ORDER, EFFECTIVE_DATE)
VALUES
  (N'MANAGE',   N'MANAGE',               110, CONVERT(DATETIME, '1990.01.01', 102)),
  (N'ORIO ACQ', N'ORIO ACQUISITION',     111, CONVERT(DATETIME, '1990.01.01', 102)),
  (N'PROP AQU', N'PROPERTY ACQUISITION', 112, CONVERT(DATETIME, '1990.01.01', 102)),
  (N'PROP MAN', N'PROPERTY MANAGEMENT',  113, CONVERT(DATETIME, '1990.01.01', 102));
GO

  