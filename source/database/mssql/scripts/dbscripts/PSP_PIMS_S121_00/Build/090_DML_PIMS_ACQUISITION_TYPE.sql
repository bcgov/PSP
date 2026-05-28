/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ACQUISITION_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
Doug Filteau  2025-Sep-04  Added CRWNTNR
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQUISITION_TYPE
GO

INSERT INTO PIMS_ACQUISITION_TYPE (ACQUISITION_TYPE_CODE, DESCRIPTION)
VALUES
  (N'CONSEN',  N'Consensual Agreement'),
  (N'SECTN3',  N'Section 3 Agreement'),
  (N'SECTN6',  N'Section 6 Expropriation'),
  (N'XFR',     N'Transferred'),
  (N'HISTORY', N'Historical'),
  (N'SECTN16', N'Land Act - Section 16 Map Reserve'),
  (N'CRWNTNR', N'Crown Tenure');
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_ACQUISITION_TYPE biz JOIN
       (SELECT ACQUISITION_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_ACQUISITION_TYPE) seq  ON seq.ACQUISITION_TYPE_CODE = biz.ACQUISITION_TYPE_CODE
GO
