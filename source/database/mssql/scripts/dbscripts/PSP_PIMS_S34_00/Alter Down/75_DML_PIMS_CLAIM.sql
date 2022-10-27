BEGIN TRANSACTION;

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

DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @documentView;
DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @documentAdd;
DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @documentEdit;
DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @documentDelete;
DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @documentAdmin;

COMMIT TRANSACTION;
