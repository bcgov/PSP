DECLARE @seedTime DateTime = CAST(N'2021-11-10T02:17:49.920' AS DateTime);
DECLARE @appUserGuid uniqueidentifier = '1b93f614-91da-4b32-b36e-bd2c6ebd12e2';
DECLARE @appUserId nvarchar(30) = N'SEED';
DECLARE @appUserDirectory nvarchar(30) = N'';
DECLARE @dbUserId nvarchar(30) = 'system';
DECLARE @dbUserGuid uniqueidentifier = null;

DECLARE @functional BIGINT;

DECLARE @activityView BIGINT;
DECLARE @activityAdd BIGINT;
DECLARE @activityEdit BIGINT;
DECLARE @activityDelete BIGINT;

DECLARE @noteDelete BIGINT;

SELECT @functional = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Functional';

SELECT @activityView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-view';
SELECT @activityAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-add';
SELECT @activityEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-edit';
SELECT @activityDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-delete';

SELECT @noteDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'note-delete';

DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityView;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityEdit;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityAdd;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityDelete;

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @noteDelete, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

GO

