/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROJECT_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROJECT_TYPE
GO

INSERT INTO PIMS_PROJECT_TYPE (PROJECT_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACQUIRE', N'Acquisition'),
  (N'DISPOSE', N'Disposition');