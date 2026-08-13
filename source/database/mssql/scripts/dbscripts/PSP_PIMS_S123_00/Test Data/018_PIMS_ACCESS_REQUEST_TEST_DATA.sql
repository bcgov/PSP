INSERT INTO PIMS_ACCESS_REQUEST (USER_ID, ROLE_ID, ACCESS_REQUEST_STATUS_TYPE_CODE, REGION_CODE, CONCURRENCY_CONTROL_NUMBER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_DIRECTORY)
VALUES
  (1, 1, 'RECEIVED',  1, 1, GETUTCDATE(), 'seed_data', 'seed_data', GETUTCDATE(), 'seed_data', 'seed_data'),
  (2, 2, 'INITIATED', 1, 1, GETUTCDATE(), 'seed_data', 'seed_data', GETUTCDATE(), 'seed_data', 'seed_data'),
  (3, 3, 'APPROVED',  1, 1, GETUTCDATE(), 'seed_data', 'seed_data', GETUTCDATE(), 'seed_data', 'seed_data'),
  (4, 4, 'DENIED',    1, 1, GETUTCDATE(), 'seed_data', 'seed_data', GETUTCDATE(), 'seed_data', 'seed_data');