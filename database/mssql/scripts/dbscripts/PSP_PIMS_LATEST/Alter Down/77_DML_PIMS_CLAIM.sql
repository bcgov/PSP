DECLARE @activityView BIGINT;
DECLARE @activityAdd BIGINT;
DECLARE @activityEdit BIGINT;
DECLARE @activityDelete BIGINT;
SELECT @activityView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-view';
SELECT @activityAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-add';
SELECT @activityEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-edit';
SELECT @activityDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-delete';

DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @activityView;
DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @activityAdd;
DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @activityEdit;
DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @activityDelete;
GO
