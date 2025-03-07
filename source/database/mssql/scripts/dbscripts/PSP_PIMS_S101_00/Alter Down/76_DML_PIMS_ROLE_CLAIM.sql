DECLARE @ltsaOwnershipView BIGINT;
DECLARE @ltsaOwnershipAdd BIGINT;
DECLARE @ltsaOwnershipEdit BIGINT;
DECLARE @ltsaOwnershipDelete BIGINT;
SELECT @ltsaOwnershipView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'ltsa-ownership-view';
SELECT @ltsaOwnershipAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'ltsa-ownership-add';
SELECT @ltsaOwnershipEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'ltsa-ownership-edit';
SELECT @ltsaOwnershipDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'ltsa-ownership-delete';

DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @ltsaOwnershipView;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @ltsaOwnershipEdit;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @ltsaOwnershipAdd;
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @ltsaOwnershipDelete;

DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @ltsaOwnershipView;
DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @ltsaOwnershipEdit;
DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @ltsaOwnershipAdd;
DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @ltsaOwnershipDelete;
