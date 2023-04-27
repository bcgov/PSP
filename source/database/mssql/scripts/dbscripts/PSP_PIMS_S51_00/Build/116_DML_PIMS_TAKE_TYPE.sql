/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_TAKE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_TAKE_TYPE
GO

INSERT INTO PIMS_TAKE_TYPE (TAKE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'PARTIAL', N'Partial'),
  (N'TOTAL',   N'Total');
GO
