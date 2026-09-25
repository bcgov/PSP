/* -----------------------------------------------------------------------------
Populate the PIMS_LEASE_PAYMENT_CATEGORY_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-May-17  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_PAYMENT_CATEGORY_TYPE
GO

INSERT INTO PIMS_LEASE_PAYMENT_CATEGORY_TYPE (LEASE_PAYMENT_CATEGORY_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'BASE', N'Base Rent',       1),
  (N'ADDL', N'Additional Rent', 2),
  (N'VBL',  N'Variable Rent',   3);
GO
