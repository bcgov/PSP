PRINT N'Adding [PIMS_PROPERTY_STATUS_TYPE]'

INSERT INTO PIMS_PROPERTY_STATUS_TYPE (PROPERTY_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ARTHWY', N'Arterial highway'),
  (N'FEESIMP', N'Fee simple'),
  (N'CROWNLND', N'Crown land'),
  (N'MOTIADMIN', N'Under MoTI administration'),
  (N'UNSURVYED', N'Unsurveyed travelled road (i.e. Section42)'),
  (N'UNDERSCRUT', N'Properties under scrutiny');
