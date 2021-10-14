/* -----------------------------------------------------------------------------
Delete all data from PIMS_ORGANIZATION table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-19  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ORGANIZATION
GO

INSERT INTO PIMS_ORGANIZATION (ORGANIZATION_IDENTIFIER, ORGANIZATION_NAME, ORGANIZATION_TYPE_CODE, ORG_IDENTIFIER_TYPE_CODE, ADDRESS_ID, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_DIRECTORY)
VALUES
  (N'MOTI', N'Ministry of Transportation and Infrastructure', N'BCMIN', N'GOV', (SELECT ADDRESS_ID FROM PIMS_ADDRESS WHERE  STREET_ADDRESS_1 LIKE '940 Blanshard%'), CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data');