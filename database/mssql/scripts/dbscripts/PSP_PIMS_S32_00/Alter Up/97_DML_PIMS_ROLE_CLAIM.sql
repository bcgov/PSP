DECLARE @restricted BIGINT;
DECLARE @finance BIGINT;
SELECT @restricted = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Functional (Restricted)';
SELECT @finance = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Finance';

DECLARE @researchAdd BIGINT;
DECLARE @leaseEdit BIGINT;
select @researchAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'researchfile-add';
select @leaseEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'lease-edit';

DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE ROLE_ID = @restricted AND CLAIM_ID = @researchAdd;

DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE ROLE_ID = @finance AND CLAIM_ID = @leaseEdit;

GO