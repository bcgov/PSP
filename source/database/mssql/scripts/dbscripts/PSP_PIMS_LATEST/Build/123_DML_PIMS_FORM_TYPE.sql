/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_FORM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jun-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_FORM_TYPE
GO

INSERT INTO PIMS_FORM_TYPE (FORM_TYPE_CODE, DESCRIPTION)
VALUES
  (N'H179P',  N'Offer agreement - Partial (H179 P)'),
  (N'H179T',  N'Offer agreement - Total (H179 T)'),
  (N'H179A',  N'Offer agreement - Section 3 (H179 A)'),
  (N'H120',   N'Payment requisition (H120)'),
  (N'H0074',  N'License of Occupation for Construction Access (H0074)'),
  (N'H0443',  N'Conditions of Entry (H0443)'),
  (N'LETTER', N'General Letter'),
  (N'FORM1',  N'Notice of Expropriation (Form 1)'),
  (N'FORM5',  N'Certificate of Approval of Expropriation (Form 5)'),
  (N'FORM8',  N'Notice of Advance Payment (Form 8)'),
  (N'FORM9',  N'Vesting Notice (Form 9)'),
  (N'H1005A', N'Licence of Occupation (BCTFA as licensor) (H1005 A)');
GO
