/* -----------------------------------------------------------------------------
Populate the PIMS_EXPROP_OWNER_HISTORY_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Feb-07  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_EXPROP_OWNER_HISTORY_TYPE
GO

INSERT INTO PIMS_EXPROP_OWNER_HISTORY_TYPE (EXPROP_OWNER_HISTORY_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'NOTCSRVDDT',   N'Expropriation notice served date',      1),
  (N'CERTEXPRAPPR', N'Certificate of expropriation approval', 2),
  (N'ADVPMTSRVDDT', N'Advanced payment served date',          3),
  (N'EXPRVSTNGDT',  N'Expropriation vesting date',            4);
GO
