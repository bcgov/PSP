-- ----------------------------------------------------------------------------------------
-- Populate the PIMS_PROP_IMPRVMNT_STATUS_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  --------------------------------------------------
-- Doug Filteau  2026-Jan-19  PSP-11151  Initial version
-- ----------------------------------------------------------------------------------------

DELETE FROM PIMS_PROP_IMPRVMNT_STATUS_TYPE
GO

INSERT INTO PIMS_PROP_IMPRVMNT_STATUS_TYPE (PROP_IMPRVMNT_STATUS_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'ACTIVE',  N'Active',   1),
  (N'ARCHIVD', N'Archived', 2);
GO
