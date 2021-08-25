PRINT N'Adding [PIMS_PROJECT_RISK_TYPE]'

INSERT INTO PIMS_PROJECT_RISK_TYPE (PROJECT_RISK_TYPE_CODE, DESCRIPTION)
VALUES
  (N'GREEN', N'90-100% of the property value'),
  (N'YELLOW', N'50% of the property value'),
  (N'RED', N'0% of the property value');
