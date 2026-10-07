-- ---------------------------------------------------------------------------------------
-- Populate the PIMS_DSP_AGREEMENT_STATUS_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -------------------------------------------------
-- Doug Filteau  2026-Jan-20  PSP-11151  Initial version
-- ---------------------------------------------------------------------------------------

DELETE FROM PIMS_DSP_AGREEMENT_STATUS_TYPE
GO

INSERT INTO PIMS_DSP_AGREEMENT_STATUS_TYPE (DSP_AGREEMENT_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'DRAFT',     N'Draft'),
  (N'FINAL',     N'Final'),
  (N'CANCELLED', N'Cancelled');
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = biz.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_DSP_AGREEMENT_STATUS_TYPE biz JOIN
       (SELECT DSP_AGREEMENT_STATUS_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_DSP_AGREEMENT_STATUS_TYPE) seq ON seq.DSP_AGREEMENT_STATUS_TYPE_CODE = biz.DSP_AGREEMENT_STATUS_TYPE_CODE
GO
