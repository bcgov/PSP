/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_AGREEMENT_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Mar-27  Initial version
Doug Filteau  2025-Aug-18  Added the H179FSPART type code
----------------------------------------------------------------------------- */

DELETE FROM PIMS_AGREEMENT_TYPE
GO

INSERT INTO PIMS_AGREEMENT_TYPE (AGREEMENT_TYPE_CODE, DESCRIPTION)
VALUES
  (N'H179T',      N'Total Agreement (H179T)'),
  (N'H179P',      N'Partial Agreement (H179P)'),
  (N'H179A',      N'Section 3 Agreement (H179A)'),
  (N'H0074',      N'License Of Occupation (H0074)'),
  (N'TOTAL',      N'Total - Fee Simple Agreement'),
  (N'H179PTO',    N'Optional Lease to Accompany H-179 (P) and H-179 (T)'),
  (N'H179FSPART', N'Agreement of Purchase and Sale H0179 (FS Part)');
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
