DECLARE @projectView BIGINT;
DECLARE @projectAdd BIGINT;
DECLARE @projectEdit BIGINT;
DECLARE @projectDelete BIGINT;
SELECT @projectView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'project-view';
SELECT @projectAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'project-add';
SELECT @projectEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'project-edit';
SELECT @projectDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'project-delete';


DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectView;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectEdit;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectAdd;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @projectDelete;

GO
