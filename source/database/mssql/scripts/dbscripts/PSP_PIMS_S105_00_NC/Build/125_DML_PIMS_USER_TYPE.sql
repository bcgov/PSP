/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_USER_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Mar-27  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_USER_TYPE
GO

INSERT INTO PIMS_USER_TYPE (USER_TYPE_CODE, DESCRIPTION)
VALUES
  (N'MINSTAFF', N'Ministry Staff'),
  (N'CONTRACT', N'Contractor');
GO
