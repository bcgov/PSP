/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_FORM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_FORM_TYPE
GO

INSERT INTO PIMS_FORM_TYPE (FORM_TYPE_CODE, DESCRIPTION)
VALUES
  (N'H179P', N'Offer agreement - Partial (H179 P)'),
  (N'H179T', N'Offer agreement - Total (H179 T)'),
  (N'H179A', N'Offer agreement - Section 3 (H179 A)'),
  (N'H120',  N'Payment requisition (H120)');
GO
