/* -----------------------------------------------------------------------------
Populate the PIMS_LAND_ACT_TYPE table.

NOTE: The LAND_ACT_TYPE_CODE values accomapny the description onscreen so the 
====  LAND_ACT_TYPE_CODE value should be formatted to be read onscreen properly.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jun-30  Initial version
Doug Filteau  2023-Nov-28  Added Crown Grant
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LAND_ACT_TYPE
GO

INSERT INTO PIMS_LAND_ACT_TYPE (LAND_ACT_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  ('Section 15',     'Order in Council Reserve',                     1),
  ('Section 16',     'Map Reserve / Withdrawal',                     2),
  ('Section 17',     'Conditional Withdrawal / Designated Use Area', 3),
  ('NOI',            'Notation of Interest',                         4),
  ('Section 66',     'Prohibited Use Area',                          5),
  ('Crown Grant',    'Crown Grant',                                  6),
  ('Transfer Admin', 'Transfer of Admin and Control',                7);
GO
