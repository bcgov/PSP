/* -----------------------------------------------------------------------------
Delete all data from the PIMS_STATIC_VARIABLE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_STATIC_VARIABLE
GO

INSERT INTO PIMS_STATIC_VARIABLE (STATIC_VARIABLE_NAME, STATIC_VARIABLE_VALUE)
VALUES
  (N'DBVERSION', N'45.01'),
  (N'GST',       N'5.0'),
  (N'FYSTART',   N'01/04/2020'),
  (N'FYEND',     N'31/03/2021');