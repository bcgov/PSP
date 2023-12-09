DECLARE @dispositionView BIGINT;
DECLARE @dispositionAdd BIGINT;
DECLARE @dispositionEdit BIGINT;
DECLARE @dispositionDelete BIGINT;
SELECT @dispositionView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'disposition-view';
SELECT @dispositionAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'disposition-add';
SELECT @dispositionEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'disposition-edit';
SELECT @dispositionDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'disposition-delete';

DECLARE @dispositionFunctional BIGINT;
DECLARE @dispositionReadOnly BIGINT;
SELECT @dispositionFunctional = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Disposition functional';
SELECT @dispositionReadOnly = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Disposition read-only';

DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @dispositionView;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @dispositionEdit;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @dispositionAdd;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @dispositionDelete;

DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE ROLE_ID = @dispositionFunctional;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE ROLE_ID = @dispositionReadOnly;

GO
