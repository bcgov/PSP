/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LESSOR_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LESSOR_TYPE
GO

INSERT INTO PIMS_LESSOR_TYPE (LESSOR_TYPE_CODE, DESCRIPTION)
VALUES
  (N'PER', N'Person'),
  (N'ORG', N'Organization');