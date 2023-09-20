DECLARE @managementView BIGINT;
DECLARE @managementAdd BIGINT;
DECLARE @managementEdit BIGINT;
DECLARE @managementDelete BIGINT;
SELECT @managementView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'management-view';
SELECT @managementAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'management-add';
SELECT @managementEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'management-edit';
SELECT @managementDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'management-delete';

DECLARE @managementFunctional BIGINT;
DECLARE @managementReadOnly BIGINT;
SELECT @managementFunctional = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Management functional';
SELECT @managementReadOnly = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Management read-only';

DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @managementView;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @managementEdit;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @managementAdd;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @managementDelete;

DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE ROLE_ID = @managementFunctional;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE ROLE_ID = @managementReadOnly;

GO
