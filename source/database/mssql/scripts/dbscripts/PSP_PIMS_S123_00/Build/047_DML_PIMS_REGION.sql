-- -------------------------------------------------------------------------------------------
-- Delete all data from the PIMS_REGION table and repopulate.
--
-- *** NOTE ***
-- The PIMS_DISTRICT table must be empty due to a foreign key dependency.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2021-Jul-09  N/A        Initial version
-- Doug Filteau  2026-Mar-13  PSP-11302  Add Headquarters region.
-- Doug Filteau  2026-Mar-30  N/A        Headquarters region disabled by Dev team.
-- -------------------------------------------------------------------------------------------

DELETE FROM PIMS_REGION;
GO

INSERT INTO PIMS_REGION (REGION_CODE, REGION_NAME, IS_DISABLED)
VALUES
  (0, N'Headquarters (HQ)',        1),
  (1, N'South Coast Region',       0),
  (2, N'Southern Interior Region', 0),
  (3, N'Northern Region',          0),
  (4, N'Cannot determine',         0);
GO
