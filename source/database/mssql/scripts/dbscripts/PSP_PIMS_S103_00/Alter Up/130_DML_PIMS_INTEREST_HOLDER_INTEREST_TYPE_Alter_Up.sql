/* -----------------------------------------------------------------------------
Order the PIMS_INTEREST_HOLDER_INTEREST_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Apr-01  Initial version.
----------------------------------------------------------------------------- */

-- --------------------------------------------------------------
-- Update the display order of all entries.
-- --------------------------------------------------------------
UPDATE prt
SET    prt.DISPLAY_ORDER              = seq.ROW_NUM
     , prt.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_INTEREST_HOLDER_INTEREST_TYPE prt JOIN
       (SELECT INTEREST_HOLDER_INTEREST_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_INTEREST_HOLDER_INTEREST_TYPE) seq  ON seq.INTEREST_HOLDER_INTEREST_TYPE_CODE = prt.INTEREST_HOLDER_INTEREST_TYPE_CODE
GO
