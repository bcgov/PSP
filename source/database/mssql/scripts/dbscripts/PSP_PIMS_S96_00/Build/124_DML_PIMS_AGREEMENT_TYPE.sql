/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_AGREEMENT_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Mar-27  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_AGREEMENT_TYPE
GO

INSERT INTO PIMS_AGREEMENT_TYPE (AGREEMENT_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'H179T', N'Total Agreement (H179T)',       1),
  (N'H179P', N'Partial Agreement (H179P)',     2),
  (N'H179A', N'Section 3 Agreement (H179A)',   3),
  (N'H0074', N'License Of Occupation (H0074)', 4);
GO
