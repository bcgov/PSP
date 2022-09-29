BEGIN TRANSACTION;

DECLARE @dbUserId nvarchar(30) = 'system';
DECLARE @dbUserGuid uniqueidentifier = null;
DECLARE @appUserId nvarchar(30) = N'SEED';
DECLARE @appUserGuid uniqueidentifier = '1b93f614-91da-4b32-b36e-bd2c6ebd12e2';
DECLARE @appUserDirectory nvarchar(30) = N'';
DECLARE @seedTime DateTime = CAST(N'2021-11-10T02:17:49.920' AS DateTime);

-- Document management claims
INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (34, N'8734f95d-b13d-46d0-99a6-4fb3fd086a56', N'document-view', N'8734f95d-b13d-46d0-99a6-4fb3fd086a56', N'Ability view existing documents.', 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId);

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (35, N'f4c0e1bf-9ac9-4765-a306-d92e4e64858b', N'document-add', N'f4c0e1bf-9ac9-4765-a306-d92e4e64858b', N'Ability to add new documents.', 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId);

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (36, N'7de91273-7fad-47ea-8cd1-f2f663a3e06d', N'document-edit', N'7de91273-7fad-47ea-8cd1-f2f663a3e06d', N'Ability to edit existing documents.', 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId);

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (37, N'e83cbf3d-a4df-4aee-9d90-ecf04e4069fd', N'document-delete', N'e83cbf3d-a4df-4aee-9d90-ecf04e4069fd', N'Ability to delete documents.', 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId);

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (38, N'646f7053-96dc-4f13-8da6-06a32e264a16', N'document-admin', N'646f7053-96dc-4f13-8da6-06a32e264a16', N'Ability to perform administrative actions on documents.', 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId);

COMMIT TRANSACTION;