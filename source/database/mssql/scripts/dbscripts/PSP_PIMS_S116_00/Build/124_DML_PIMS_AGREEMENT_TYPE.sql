-- -------------------------------------------------------------------------------------------
-- Populate the missing code values in the PIMS_AGREEMENT_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2023-Mar-27  N/A        Initial version
-- Doug Filteau  2025-Aug-18  N/A        Added the H179FSPART type code
-- Doug Filteau  2026-Jan-19  PSP-11164  Added the H179B and H179D type code
-- -------------------------------------------------------------------------------------------

DELETE FROM PIMS_AGREEMENT_TYPE
GO

INSERT INTO PIMS_AGREEMENT_TYPE (AGREEMENT_TYPE_CODE, DESCRIPTION)
VALUES
  (N'H179T',      N'Total Agreement (H179T)'),
  (N'H179P',      N'Partial Agreement (H179P)'),
  (N'H179A',      N'Section 3 Agreement (H179A)'),
  (N'H0074',      N'License Of Occupation (H0074)'),
  (N'H179FS',     N'Fee Simple Agreement (H179FS)'),
  (N'H179PTO',    N'Optional Lease to Accompany H-179 (P) and H-179 (T)'),
  (N'H179FSPART', N'Agreement of Purchase and Sale H0179 (FS Part)'),
  (N'H179B',      N'Release of Claims Agreement (H179B)'),
  (N'H179D',      N'Statutory Right of Way Agreement (H179D)'),
  (N'H179RC',     N'Agreement of Purchase and Sale (Closed Road) (H179RC)');
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_AGREEMENT_TYPE biz JOIN
       (SELECT AGREEMENT_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_AGREEMENT_TYPE) seq  ON seq.AGREEMENT_TYPE_CODE = biz.AGREEMENT_TYPE_CODE
GO
