INSERT INTO PIMS_ROLE (ROLE_UID, NAME, DESCRIPTION, IS_PUBLIC, IS_DISABLED, SORT_ORDER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_DIRECTORY)
VALUES
  (NEWID(), N'Disposition functional',     N'Access to create, read, update Disposition files.',       CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Disposition read-only',      N'Access to read Disposition files',                        CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data');

GO
