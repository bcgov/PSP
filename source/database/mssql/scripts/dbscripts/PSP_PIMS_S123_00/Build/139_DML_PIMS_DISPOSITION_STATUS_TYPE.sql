/* -----------------------------------------------------------------------------
Populate the PIMS_DISPOSITION_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Nov-21  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DISPOSITION_STATUS_TYPE
GO

INSERT INTO PIMS_DISPOSITION_STATUS_TYPE (DISPOSITION_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'UNKNOWN',   N'Unknown'),
  (N'PREMARKET', N'Pre-Marketing'),
  (N'LISTED',    N'Listed'),
  (N'PENDING',   N'Pending Sale'),
  (N'SOLD',      N'Sold'),
  (N'ONHOLD',    N'On Hold'),
  (N'ACTIVE',    N'Active');
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_DISPOSITION_STATUS_TYPE biz JOIN
       (SELECT DISPOSITION_STATUS_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_DISPOSITION_STATUS_TYPE) seq  ON seq.DISPOSITION_STATUS_TYPE_CODE = biz.DISPOSITION_STATUS_TYPE_CODE
GO
