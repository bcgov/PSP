DECLARE @restricted BIGINT;
SELECT @restricted = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Functional (Restricted)';

DECLARE @researchAdd BIGINT;
select @researchAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'researchfile-add';

DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE ROLE_ID = @restricted AND CLAIM_ID = @researchAdd;

GO