BEGIN TRANSACTION;

DECLARE @dbClaimsUserId nvarchar(30) = 'system';
DECLARE @dbClaimsUserGuid uniqueidentifier = null;
DECLARE @appClaimsUserId nvarchar(30) = N'SEED';
DECLARE @appClaimsUserGuid uniqueidentifier = '1b93f614-91da-4b32-b36e-bd2c6ebd12e2';
DECLARE @appClaimsUserDirectory nvarchar(30) = N'';
DECLARE @seedClaimsTime DateTime = GETDATE();

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (69, N'dce955aa-e387-4c61-820d-232ae273f16e', N'notification-view', N'dce955aa-e387-4c61-820d-232ae273f16e', N'Ability to view Notifications.', 0, 1, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId);

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (70, N'27603b6a-aec0-4b33-aad8-7a6d6c7d8527', N'notification-add', N'27603b6a-aec0-4b33-aad8-7a6d6c7d8527', N'Ability to add new Notification.', 0, 1, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId);

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (71, N'c4771de3-3cf1-4200-a73d-1fe05daaa3b9', N'notification-edit', N'c4771de3-3cf1-4200-a73d-1fe05daaa3b9', N'Ability to edit a Notification.', 0, 1, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId);

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (72, N'8eab5459-08f5-4c0f-8344-f1ecf61e4e01', N'notification-delete', N'8eab5459-08f5-4c0f-8344-f1ecf61e4e01', N'Ability to delete a Notification.', 0, 1, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId);

COMMIT TRANSACTION;
