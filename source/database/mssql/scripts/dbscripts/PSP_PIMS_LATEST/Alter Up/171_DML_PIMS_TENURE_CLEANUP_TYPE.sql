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
  (N'PROPMGR',  N'Section 42 roads (maybe)'),
  (N'TBD',      N'TBD');
GO
