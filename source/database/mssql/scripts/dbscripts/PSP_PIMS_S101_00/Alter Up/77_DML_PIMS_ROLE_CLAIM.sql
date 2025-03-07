
DECLARE @dbUserId nvarchar(30) = 'system';
DECLARE @dbUserGuid uniqueidentifier = null;
DECLARE @appUserId nvarchar(30) = N'SEED';
DECLARE @appUserGuid uniqueidentifier = '1b93f614-91da-4b32-b36e-bd2c6ebd12e2';
DECLARE @appUserDirectory nvarchar(30) = N'';
DECLARE @seedTime DateTime = GETDATE();

DECLARE @systemAdministrator BIGINT;
SELECT @systemAdministrator = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'System Administrator';

-- ****************************************************************************
-- Declare and initialize the claims
-- ****************************************************************************
DECLARE @ltsaOwnershipView BIGINT;
DECLARE @ltsaOwnershipAdd BIGINT;
DECLARE @ltsaOwnershipEdit BIGINT;
DECLARE @ltsaOwnershipDelete BIGINT;
--
SELECT @ltsaOwnershipView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'ltsa-ownership-view';
SELECT @ltsaOwnershipAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'ltsa-ownership-add';
SELECT @ltsaOwnershipEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'ltsa-ownership-edit';
SELECT @ltsaOwnershipDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'ltsa-ownership-delete';
-- ----------------------------------------------------------------------------

/* System Administrator */
INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@systemAdministrator, @ltsaOwnershipView, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@systemAdministrator, @ltsaOwnershipEdit, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@systemAdministrator, @ltsaOwnershipAdd, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@systemAdministrator, @ltsaOwnershipDelete, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)
