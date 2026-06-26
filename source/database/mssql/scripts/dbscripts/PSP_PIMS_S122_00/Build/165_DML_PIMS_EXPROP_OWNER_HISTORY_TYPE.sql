-- -------------------------------------------------------------------------------------------
-- Populate the PIMS_EXPROP_OWNER_HISTORY_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2025-Feb-07  N/A        Initial version
-- Doug Filteau  2026-Mar-13  PSP-11293  Added APPEFFCTVDT.
-- -------------------------------------------------------------------------------------------

DELETE FROM PIMS_EXPROP_OWNER_HISTORY_TYPE
GO

INSERT INTO PIMS_EXPROP_OWNER_HISTORY_TYPE (EXPROP_OWNER_HISTORY_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'NOTCSRVDDT',   N'Expropriation notice served date',      1),
  (N'CERTEXPRAPPR', N'Certificate of expropriation approval', 2),
  (N'ADVPMTSRVDDT', N'Advanced payment served date',          3),
  (N'EXPRVSTNGDT',  N'Expropriation vesting date',            4),
  (N'APPEFFCTVDT',  N'Appraisal effective date',              5);
GO
