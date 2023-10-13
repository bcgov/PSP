/* -----------------------------------------------------------------------------
Populate the PIMS_LAND_ACT_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-May-01  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LAND_ACT_TYPE
GO

INSERT INTO PIMS_LAND_ACT_TYPE (LAND_ACT_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  ('Section 15', 'Order in Council Reserve',                     1),
  ('Section 16', 'Map Reserve / Withdrawal',                     2),
  ('Section 17', 'Conditional Withdrawal / Designated Use Area', 3),
  ('NOI',        'Notation of Interest',                         4),
  ('Section 66', 'Prohibited Use Area',                          5);
GO
