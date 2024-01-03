DECLARE @dispositionView BIGINT;
DECLARE @dispositionAdd BIGINT;
DECLARE @dispositionEdit BIGINT;
DECLARE @dispositionDelete BIGINT;
SELECT @dispositionView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'disposition-view';
SELECT @dispositionAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'disposition-add';
SELECT @dispositionEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'disposition-edit';
SELECT @dispositionDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'disposition-delete';


DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @dispositionView;
DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @dispositionEdit;
DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @dispositionAdd;
DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @dispositionDelete;

GO
