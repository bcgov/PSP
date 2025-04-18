/* -----------------------------------------------------------------------------
Order the PIMS_INTEREST_HOLDER_INTEREST_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Apr-01  Initial version.
----------------------------------------------------------------------------- */

-- --------------------------------------------------------------
-- Nullify the display order of all entries.
-- --------------------------------------------------------------
UPDATE PIMS_INTEREST_HOLDER_INTEREST_TYPE
SET    DISPLAY_ORDER              = NULL
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_INTEREST_HOLDER_INTEREST_TYPE
GO
