-- -------------------------------------------------------------------------------------------
-- Populate the PIMS_DISPOSITION_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Doug Filteau  2023-Nov-21  N/A        Initial version.
-- Doug Filteau  2026-Jan-19  PSP-11141  Added the SRW code.
-- -------------------------------------------------------------------------------------------

DELETE FROM PIMS_DISPOSITION_TYPE
GO

INSERT INTO PIMS_DISPOSITION_TYPE (DISPOSITION_TYPE_CODE, DESCRIPTION)
VALUES
  (N'OPEN',    N'Open Market Sale'),
  (N'DIRECT',  N'Direct Sale'),
  (N'CLOSURE', N'Road Closure'),
  (N'OTHER',   N'Other Transfer'),
  (N'SRW',     N'Statutory Right of Way');
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = biz.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_DISPOSITION_TYPE biz JOIN
       (SELECT DISPOSITION_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_DISPOSITION_TYPE) seq ON seq.DISPOSITION_TYPE_CODE = biz.DISPOSITION_TYPE_CODE
GO
