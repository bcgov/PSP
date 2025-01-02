/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LETTER_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LETTER_TYPE
GO

INSERT INTO PIMS_LETTER_TYPE (LETTER_TYPE_CODE, DESCRIPTION)
VALUES
  (N'OWNER', N'Owner Letter');
GO
