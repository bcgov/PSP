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
    (65, N'db03a38d-4eb4-41b1-b4f4-31e3355453f7', N'ltsa-ownership-view', N'db03a38d-4eb4-41b1-b4f4-31e3355453f7', N'Ability to view LTSA ownership information.', 0, 1, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId);

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (66, N'17384997-7c34-407f-a896-66d599cb7bc7', N'ltsa-ownership-add', N'17384997-7c34-407f-a896-66d599cb7bc7', N'Ability to add new LTSA ownership information.', 0, 1, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId);

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (67, N'209b2ed6-2212-41e7-8308-fa0c822e78ca', N'ltsa-ownership-edit', N'209b2ed6-2212-41e7-8308-fa0c822e78ca', N'Ability to edit existing LTSA ownership information.', 0, 1, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId);

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (68, N'3866098e-e3ea-4527-935e-517c1e33e771', N'ltsa-ownership-delete', N'3866098e-e3ea-4527-935e-517c1e33e771', N'Ability to delete LTSA ownership information.', 0, 1, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId, @seedClaimsTime, @appClaimsUserId, @appClaimsUserGuid, @appClaimsUserDirectory, @seedClaimsTime, @dbClaimsUserId);

COMMIT TRANSACTION;
