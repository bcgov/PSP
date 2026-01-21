/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_WORK_ACTIVITY_CODE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_WORK_ACTIVITY_CODE
GO

INSERT INTO PIMS_WORK_ACTIVITY_CODE (CODE, DESCRIPTION, DISPLAY_ORDER, EFFECTIVE_DATE)
VALUES
  (N'PROP MAN',   N'PROP MAN',   2000, CONVERT(DATETIME, '1990.01.01', 102)),
  (N'SURVEY',     N'SURVEY',     2001, CONVERT(DATETIME, '1990.01.01', 102)),
  (N'ELEC MAIN',  N'ELEC MAIN',  2002, CONVERT(DATETIME, '1990.01.01', 102)),
  (N'ACQ PROP',   N'ACQ PROP',   2003, CONVERT(DATETIME, '1990.01.01', 102)),
  (N'PA GENERAL', N'PA GENERAL', 2004, CONVERT(DATETIME, '1990.01.01', 102));
  