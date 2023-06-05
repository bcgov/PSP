-- assign claims to all new roles.

DECLARE @appUserGuid uniqueidentifier = NEWID();

-- Declare and initialize the roles
DECLARE @llfunc  BIGINT;
SELECT @llfunc  = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Lease/License functional';

DECLARE @sysadmn BIGINT;
SELECT @sysadmn = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'System administrator';

DECLARE @resfunc  BIGINT;
SELECT @resfunc = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Research functional';

-- Declare and initialize the claims
DECLARE @projectView BIGINT;
SELECT @projectView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'project-view';

DECLARE @activityView BIGINT;
DECLARE @activityAdd BIGINT;
DECLARE @activityEdit BIGINT;
DECLARE @activityDelete BIGINT;
SELECT @activityView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-view';
SELECT @activityAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-add';
SELECT @activityEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-edit';
SELECT @activityDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-delete';

INSERT INTO [dbo].[PIMS_ROLE_CLAIM] ([ROLE_ID], [CLAIM_ID], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [APP_LAST_UPDATE_USER_DIRECTORY])
VALUES
    (@sysadmn,    @activityView,        N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@sysadmn,    @activityEdit,        N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@sysadmn,    @activityAdd,         N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@sysadmn,    @activityDelete,      N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', '');

DELETE FROM PIMS_ROLE_CLAIM WHERE ROLE_ID = @llfunc AND CLAIM_ID = @projectView;
DELETE FROM PIMS_ROLE_CLAIM WHERE ROLE_ID = @resfunc AND CLAIM_ID = @projectView;