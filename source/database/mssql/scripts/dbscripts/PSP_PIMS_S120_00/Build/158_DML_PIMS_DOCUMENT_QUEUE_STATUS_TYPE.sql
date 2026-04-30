/* -----------------------------------------------------------------------------
Populate the PIMS_DOCUMENT_QUEUE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Oct-17  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DOCUMENT_QUEUE_STATUS_TYPE
GO

INSERT INTO PIMS_DOCUMENT_QUEUE_STATUS_TYPE (DOCUMENT_QUEUE_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'PENDING',     'Pending'),
  (N'PROCESSING',  'Processing'),
  (N'SUCCESS',     'Success'),
  (N'PIMS_ERROR',  'PIMS Error'),
  (N'MAYAN_ERROR', 'MAYAN Error');
GO

-- --------------------------------------------------------------
-- Update the display order with the exception of the OTHER type.
-- --------------------------------------------------------------
UPDATE prnt
SET    prnt.DISPLAY_ORDER              = chld.ROW_NUM
     , prnt.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_DOCUMENT_QUEUE_STATUS_TYPE prnt JOIN
       (SELECT DOCUMENT_QUEUE_STATUS_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_DOCUMENT_QUEUE_STATUS_TYPE) chld ON chld.DOCUMENT_QUEUE_STATUS_TYPE_CODE = prnt.DOCUMENT_QUEUE_STATUS_TYPE_CODE
GO
