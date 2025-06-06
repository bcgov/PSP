DECLARE @formView BIGINT;
DECLARE @formAdd BIGINT;
DECLARE @formEdit BIGINT;
DECLARE @formDelete BIGINT;
SELECT @formView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'form-view';
SELECT @formAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'form-add';
SELECT @formEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'form-edit';
SELECT @formDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'form-delete';


DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @formView;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @formEdit;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @formAdd;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @formDelete;

GO