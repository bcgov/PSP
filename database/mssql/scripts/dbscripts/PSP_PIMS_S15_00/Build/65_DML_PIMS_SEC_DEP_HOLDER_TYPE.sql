/* -----------------------------------------------------------------------------
Delete all data from the PIMS_SEC_DEP_HOLDER_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_SEC_DEP_HOLDER_TYPE
GO

INSERT INTO PIMS_SEC_DEP_HOLDER_TYPE (SEC_DEP_HOLDER_TYPE_CODE, DESCRIPTION)
VALUES
  (N'MINISTRY', N'Ministry'),
  (N'PROPMGR',  N'Property Manager'),
  (N'OTHER',    N'Other');