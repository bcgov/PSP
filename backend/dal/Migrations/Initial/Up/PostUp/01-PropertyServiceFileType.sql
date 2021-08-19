PRINT N'Adding [PIMS_PROPERTY_SERVICE_FILE_TYPE]'

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
