BEGIN TRANSACTION;

DECLARE @functional BIGINT;
DECLARE @restricted BIGINT;
DECLARE @readOnly BIGINT;
DECLARE @systemAdministrator BIGINT;
SELECT @functional = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Functional';
SELECT @restricted = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Functional (Restricted)';
SELECT @readOnly = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Read Only';
SELECT @systemAdministrator = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'System Administrator';

DECLARE @documentView BIGINT;
DECLARE @documentAdd BIGINT;
DECLARE @documentEdit BIGINT;
DECLARE @documentDelete BIGINT;
DECLARE @documentAdmin BIGINT;
SELECT @documentView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'document-view';
SELECT @documentAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'document-add';
SELECT @documentEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'document-edit';
SELECT @documentDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'document-delete';
SELECT @documentAdmin = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'document-admin';

/* Administrator */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @documentView AND ROLE_ID = @systemAdministrator;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @documentEdit AND ROLE_ID = @systemAdministrator;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @documentAdd AND ROLE_ID = @systemAdministrator;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @documentDelete AND ROLE_ID = @systemAdministrator;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @documentAdmin AND ROLE_ID = @systemAdministrator;

/* Functional */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @documentView AND ROLE_ID = @functional;    
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @documentEdit AND ROLE_ID = @functional;    
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @documentAdd AND ROLE_ID = @functional;    
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @documentDelete AND ROLE_ID = @functional;    

/* Functional Restricted */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @documentView AND ROLE_ID = @restricted;    
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @documentEdit AND ROLE_ID = @restricted;    
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @documentAdd AND ROLE_ID = @restricted; 

/* Read Only */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @documentView AND ROLE_ID = @readOnly;   

COMMIT TRANSACTION;
