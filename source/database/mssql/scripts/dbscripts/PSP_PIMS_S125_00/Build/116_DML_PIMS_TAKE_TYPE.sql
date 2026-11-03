/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_TAKE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version.
Doug Filteau  2024-Apr-18  Add 'IMPORTED' take type.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_TAKE_TYPE
GO

INSERT INTO PIMS_TAKE_TYPE (TAKE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'IMPORTED', N'Imported', 1),
  (N'PARTIAL',  N'Partial',  0),
  (N'TOTAL',    N'Total',    0);
GO
