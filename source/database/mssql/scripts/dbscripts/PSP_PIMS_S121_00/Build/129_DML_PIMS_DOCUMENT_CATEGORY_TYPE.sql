/* -----------------------------------------------------------------------------
Populate the PIMS_DOCUMENT_CATEGORY_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-May-18  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DOCUMENT_CATEGORY_TYPE
GO

INSERT INTO PIMS_DOCUMENT_CATEGORY_TYPE (DOCUMENT_CATEGORY_TYPE_CODE, DESCRIPTION)
VALUES
  (N'PROJECT',    N'Project'),
  (N'RESEARCH',   N'Research file'),
  (N'ACQUIRE',    N'Acquisition file'),
  (N'LEASLIC',    N'Lease/License'),
  (N'DISPOSE',    N'Disposition file'),
  (N'MANAGEMENT', N'Management file');
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE pmat
SET    pmat.DISPLAY_ORDER              = seq.ROW_NUM
     , pmat.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_DOCUMENT_CATEGORY_TYPE pmat JOIN
       (SELECT DOCUMENT_CATEGORY_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_DOCUMENT_CATEGORY_TYPE) seq  ON seq.DOCUMENT_CATEGORY_TYPE_CODE = pmat.DOCUMENT_CATEGORY_TYPE_CODE
GO
