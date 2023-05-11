BEGIN TRANSACTION;

DECLARE @dbUserId nvarchar(30) = 'system';
DECLARE @dbUserGuid uniqueidentifier = null;
DECLARE @appUserId nvarchar(30) = N'SEED';
DECLARE @appUserGuid uniqueidentifier = '1b93f614-91da-4b32-b36e-bd2c6ebd12e2';
DECLARE @appUserDirectory nvarchar(30) = N'';
DECLARE @seedTime DateTime = GETDATE();

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (53, N'10f130bc-2202-4988-8652-6e88af04ecc4', N'compensation-requisition-view', N'10f130bc-2202-4988-8652-6e88af04ecc4', N'Ability to view Acquisition File compensation requisitions.', 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId);

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (54, N'c4948eaa-3f90-43fc-8c71-8e72cc33d03d', N'compensation-requisition-add', N'c4948eaa-3f90-43fc-8c71-8e72cc33d03d', N'Ability to add new Acquisition File compensation requisitions.', 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId);

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (55, N'86b6893c-a4d9-4d40-847b-4ff107734441', N'compensation-requisition-edit', N'86b6893c-a4d9-4d40-847b-4ff107734441', N'Ability to edit existing Acquisition File compensation requisitions.', 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId);

INSERT [dbo].[PIMS_CLAIM]
    ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (56, N'b175aa83-cee0-4cc3-907c-0c8e66610224', N'compensation-requisition-delete', N'b175aa83-cee0-4cc3-907c-0c8e66610224', N'Ability to delete Acquisition File compensation requisitions.', 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId);

COMMIT TRANSACTION;
