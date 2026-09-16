-- Creating test data for surplus declerations
INSERT INTO PIMS_PROPERTY_LEASE
  (PROPERTY_ID, LEASE_ID, CONCURRENCY_CONTROL_NUMBER)
VALUES
  (2, 45, 1)
INSERT INTO PIMS_PROPERTY_LEASE
  (PROPERTY_ID, LEASE_ID, CONCURRENCY_CONTROL_NUMBER)
VALUES
  (3, 45, 1)

UPDATE PIMS_PROPERTY SET SURPLUS_DECLARATION_TYPE_CODE = 'YES', SURPLUS_DECLARATION_DATE = CURRENT_TIMESTAMP, CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE PROPERTY_ID = 39

UPDATE PIMS_PROPERTY SET SURPLUS_DECLARATION_TYPE_CODE = 'UNKNOWN', SURPLUS_DECLARATION_DATE = CURRENT_TIMESTAMP, SURPLUS_DECLARATION_COMMENT = 'Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Vestibulum tempus iaculis ex eget semper. Etiam eget sollicitudin mauris. Mauris nec ligula eu massa faucibus ullamcorper in a dui. Donec ut arcu lacinia libero vehicula convallis vel tempus orci. Integer erat turpis, rhoncus et auctor rhoncus, blandit at elit. In elit lectus, finibus non interdum ut, sodales nec leo. In orci odio, finibus in mi et, rhoncus blandit ligula.',CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE PROPERTY_ID = 2

UPDATE PIMS_PROPERTY SET SURPLUS_DECLARATION_TYPE_CODE = 'NO', SURPLUS_DECLARATION_DATE = CURRENT_TIMESTAMP, SURPLUS_DECLARATION_COMMENT = 'Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Vestibulum tempus iaculis ex eget semper. Etiam eget sollicitudin mauris. Mauris nec ligula eu massa faucibus ullamcorper in a dui. Donec ut arcu lacinia libero vehicula convallis vel tempus orci. Integer erat turpis, rhoncus et auctor rhoncus, blandit at elit. In elit lectus, finibus non interdum ut, sodales nec leo. In orci odio, finibus in mi et, rhoncus blandit ligula.',CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE PROPERTY_ID = 3
