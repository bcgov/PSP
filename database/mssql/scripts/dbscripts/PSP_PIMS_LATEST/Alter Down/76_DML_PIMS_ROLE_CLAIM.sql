DECLARE @finance BIGINT;
DECLARE @functional BIGINT;
DECLARE @restricted BIGINT;
DECLARE @readOnly BIGINT;
DECLARE @systemAdministrator BIGINT;
SELECT @finance = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Finance';
SELECT @functional = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Functional';
SELECT @restricted = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Functional (Restricted)';
SELECT @readOnly = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Read Only';
SELECT @systemAdministrator = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'System Administrator';

DECLARE @activityView BIGINT;
DECLARE @activityAdd BIGINT;
DECLARE @activityEdit BIGINT;
DECLARE @activityDelete BIGINT;
SELECT @activityView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-view';
SELECT @activityAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-add';
SELECT @activityEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-edit';
SELECT @activityDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-delete';

/* Functional */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityView AND ROLE_ID = @functional;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityEdit AND ROLE_ID = @functional;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityAdd AND ROLE_ID = @functional;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityDelete AND ROLE_ID = @functional;

/* Functional Restricted */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityView AND ROLE_ID = @restricted;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityEdit AND ROLE_ID = @restricted;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityAdd AND ROLE_ID = @restricted;

/* Read Only */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityView AND ROLE_ID = @readOnly;

/* System Administrator */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityView AND ROLE_ID = @systemAdministrator;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityEdit AND ROLE_ID = @systemAdministrator;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityAdd AND ROLE_ID = @systemAdministrator;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @activityDelete AND ROLE_ID = @systemAdministrator;
GO
