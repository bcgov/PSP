/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_SERVICE_FILE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_SERVICE_FILE_TYPE
GO

INSERT INTO PIMS_PROPERTY_SERVICE_FILE_TYPE (PROPERTY_SERVICE_FILE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'WATERTEST', N'Test Water'),
  (N'MUNGARBAGE', N'Municipal Garbage'),
  (N'ACCESS', N'Access'),
  (N'NOACCESS', N'No Access'),
  (N'MUNWATER', N'Municipal Water'),
  (N'ELECTRIC', N'Electricity'),
  (N'MUNSEWER', N'Municipal Sewer'),
  (N'PHONE', N'Phone'),
  (N'GAS', N'Gas'),
  (N'SEPTIC', N'Septic'),
  (N'WELL', N'Well');