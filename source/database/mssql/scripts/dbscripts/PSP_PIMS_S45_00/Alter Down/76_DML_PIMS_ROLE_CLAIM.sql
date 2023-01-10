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

DECLARE @projectView BIGINT;
DECLARE @projectAdd BIGINT;
DECLARE @projectEdit BIGINT;
DECLARE @projectDelete BIGINT;
SELECT @projectView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'project-view';
SELECT @projectAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'project-add';
SELECT @projectEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'project-edit';
SELECT @projectDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'project-delete';

/* Functional */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectView AND ROLE_ID = @functional;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectEdit AND ROLE_ID = @functional;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectAdd AND ROLE_ID = @functional;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectDelete AND ROLE_ID = @functional;

/* System Administrator */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectView AND ROLE_ID = @systemAdministrator;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectEdit AND ROLE_ID = @systemAdministrator;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectAdd AND ROLE_ID = @systemAdministrator;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectDelete AND ROLE_ID = @systemAdministrator;

/* Functional Restricted */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectView AND ROLE_ID = @restricted;

/* Finance */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectView AND ROLE_ID = @finance;

/* Read Only */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectView AND ROLE_ID = @readOnly;

GO
