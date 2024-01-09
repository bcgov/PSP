/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_DSP_CHKLST_ITEM_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Dec-04  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DSP_CHKLST_ITEM_STATUS_TYPE
GO

INSERT INTO PIMS_DSP_CHKLST_ITEM_STATUS_TYPE (DSP_CHKLST_ITEM_STATUS_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'INCOMP', N'Incomplete',     1),
  (N'COMPLT', N'Complete',       2),
  (N'NOTAPP', N'Not applicable', 3);
GO
