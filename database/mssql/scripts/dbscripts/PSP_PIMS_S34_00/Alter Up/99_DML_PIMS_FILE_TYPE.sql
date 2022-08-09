/* -----------------------------------------------------------------------------
Delete all data from the PIMS_FILE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_FILE_TYPE
GO

INSERT INTO PIMS_FILE_TYPE (FILE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'GENERAL',  N'General'),
  (N'ACQUIRE',  N'Acquisitiion'),
  (N'DISPOSE',  N'Disposition'),
  (N'RSEARCH',  N'Research');
GO
