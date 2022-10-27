/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ORGANIZATION table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ORGANIZATION
GO

INSERT INTO PIMS_ORGANIZATION (
    ORGANIZATION_IDENTIFIER
  , ORGANIZATION_NAME
  , ORGANIZATION_TYPE_CODE
  , ORG_IDENTIFIER_TYPE_CODE
  , IS_DISABLED
  , APP_CREATE_TIMESTAMP
  , APP_CREATE_USERID
  , APP_CREATE_USER_DIRECTORY
  , APP_LAST_UPDATE_TIMESTAMP
  , APP_LAST_UPDATE_USERID
  , APP_LAST_UPDATE_USER_DIRECTORY)
VALUES (
    N'MOTI2'
  , N'Ministry of Transportation and Infrastructure'
  , N'BCMIN'
  , N'GOV'
  , 0
  , CURRENT_TIMESTAMP
  , N'Seed Data'
  , N'Seed Data'
  , CURRENT_TIMESTAMP
  , N'Seed Data'
  , N'Seed Data');

