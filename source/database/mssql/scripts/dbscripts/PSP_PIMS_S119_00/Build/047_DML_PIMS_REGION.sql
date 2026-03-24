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
-- -------------------------------------------------------------------------------------------

DELETE FROM PIMS_REGION;
GO

INSERT INTO PIMS_REGION (REGION_CODE, REGION_NAME) 
VALUES
  (0, N'Headquarters (HQ)'),
  (1, N'South Coast Region'),
  (2, N'Southern Interior Region'),
  (3, N'Northern Region'),
  (4, N'Cannot determine');
GO
