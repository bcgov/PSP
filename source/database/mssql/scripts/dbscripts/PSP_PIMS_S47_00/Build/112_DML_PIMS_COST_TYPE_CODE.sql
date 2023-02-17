/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_COST_TYPE_CODE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_COST_TYPE_CODE
GO

INSERT INTO PIMS_COST_TYPE_CODE (CODE, DESCRIPTION, DISPLAY_ORDER, EFFECTIVE_DATE)
VALUES
  (N'WBO', N'WBO', 1000, CONVERT(DATETIME, '1990.01.01', 102));
  