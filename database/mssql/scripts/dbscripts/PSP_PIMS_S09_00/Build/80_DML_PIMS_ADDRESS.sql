/* -----------------------------------------------------------------------------
Initial seed of the PIMS_ADDRESS table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-19  Initial version
----------------------------------------------------------------------------- */

INSERT INTO PIMS_ADDRESS (STREET_ADDRESS_1, MUNICIPALITY_NAME, POSTAL_CODE, PROVINCE_STATE_ID, COUNTRY_ID, ADDRESS_USAGE_TYPE_CODE, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_DIRECTORY)
VALUES
  (N'940 Blanshard Street', N'Victoria', N'V8W 3E6', 1, 1, N'MAILADDR', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data');