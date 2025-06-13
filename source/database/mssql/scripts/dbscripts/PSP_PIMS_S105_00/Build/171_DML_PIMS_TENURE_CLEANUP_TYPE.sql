/* -----------------------------------------------------------------------------
Populate the PIMS_TENURE_CLEANUP_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-May-23  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_TENURE_CLEANUP_TYPE
GO

INSERT INTO PIMS_TENURE_CLEANUP_TYPE (TENURE_CLEANUP_TYPE_CODE, DESCRIPTION)
VALUES
  (N'NEEDSRVY', N'Needs Survey'),
  (N'FORM12',   N'Form 12'),
  (N'SECT42',   N'Section 42 roads (maybe)'),
  (N'TBD',      N'TBD');
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE main
SET    main.DISPLAY_ORDER              = seq.ROW_NUM
     , main.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_TENURE_CLEANUP_TYPE main JOIN
       (SELECT TENURE_CLEANUP_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_TENURE_CLEANUP_TYPE) seq  ON seq.TENURE_CLEANUP_TYPE_CODE = main.TENURE_CLEANUP_TYPE_CODE
GO
