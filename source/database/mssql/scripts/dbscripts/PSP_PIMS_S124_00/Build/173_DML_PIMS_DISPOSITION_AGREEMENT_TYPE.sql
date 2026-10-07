-- ---------------------------------------------------------------------------------------
-- Populate the missing code values in the PIMS_DISPOSITION_AGREEMENT_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -------------------------------------------------
-- Doug Filteau  2026-Jan-20  PSP-11151  Initial version
-- ---------------------------------------------------------------------------------------

DELETE FROM PIMS_DISPOSITION_AGREEMENT_TYPE
GO

INSERT INTO PIMS_DISPOSITION_AGREEMENT_TYPE (DISPOSITION_AGREEMENT_TYPE_CODE, DESCRIPTION)
VALUES
  (N'H179RC', N'Agreement of Purchase and Sale (Closed Road) (H179RC)');
GO
