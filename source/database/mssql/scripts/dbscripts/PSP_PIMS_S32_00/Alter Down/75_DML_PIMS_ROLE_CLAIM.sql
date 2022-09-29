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

DECLARE @noteView BIGINT;
DECLARE @noteAdd BIGINT;
DECLARE @noteEdit BIGINT;
DECLARE @noteDelete BIGINT;
SELECT @noteView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'note-view';
SELECT @noteAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'note-add';
SELECT @noteEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'note-edit';
SELECT @noteDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'note-delete';

/* Functional */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @noteView AND ROLE_ID = @functional;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @noteEdit AND ROLE_ID = @functional;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @noteAdd AND ROLE_ID = @functional;

/* Functional Restricted */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @noteView AND ROLE_ID = @restricted;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @noteEdit AND ROLE_ID = @restricted;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @noteAdd AND ROLE_ID = @restricted;

/* Read Only */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @noteView AND ROLE_ID = @readOnly;

/* System Administrator */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @noteView AND ROLE_ID = @systemAdministrator;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @noteEdit AND ROLE_ID = @systemAdministrator;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @noteAdd AND ROLE_ID = @systemAdministrator;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @noteDelete AND ROLE_ID = @systemAdministrator;
GO
