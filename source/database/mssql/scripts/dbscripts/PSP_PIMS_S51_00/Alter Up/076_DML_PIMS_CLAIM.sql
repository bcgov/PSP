BEGIN TRANSACTION;

DECLARE @dbUserId nvarchar(30) = 'system';
DECLARE @dbUserGuid uniqueidentifier = null;
DECLARE @appUserId nvarchar(30) = N'SEED';
DECLARE @appUserGuid uniqueidentifier = '1b93f614-91da-4b32-b36e-bd2c6ebd12e2';
DECLARE @appUserDirectory nvarchar(30) = N'';
DECLARE @seedTime DateTime = CAST(N'2023-04-01T12:00:00.0' AS DateTime);

-- Agreement claims
INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (52, N'7bad14e8-03c7-49c5-aa94-650128a734f3', N'agreement-view', N'7bad14e8-03c7-49c5-aa94-650128a734f3', N'Ability to view agreements.', 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId);

COMMIT TRANSACTION;