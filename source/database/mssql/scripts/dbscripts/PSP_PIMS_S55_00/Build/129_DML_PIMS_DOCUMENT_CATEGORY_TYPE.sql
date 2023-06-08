/* -----------------------------------------------------------------------------
Populate the PIMS_DOCUMENT_CATEGORY_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-May-18  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DOCUMENT_CATEGORY_TYPE
GO

INSERT INTO PIMS_DOCUMENT_CATEGORY_TYPE (DOCUMENT_CATEGORY_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  ('PROJECT',  'Project',          1),
  ('RESEARCH', 'Research file',    2),
  ('ACQUIRE',  'Acquisition file', 3),
  ('LEASLIC',  'Lease/License',    4),
  ('DISPOSE',  'Disposition file', 5);
GO
