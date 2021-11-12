declare @view bigint,@add bigint,@edit bigint,@delete bigint;
SELECT @view=CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'contact-view';
SELECT @add=CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'contact-add';
SELECT @edit=CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'contact-edit';
SELECT @delete=CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'contact-delete';

DELETE FROM dbo.[PIMS_ROLE_CLAIM] where Claim_Id in (@view,@add,@edit,@delete)
DELETE FROM dbo.[PIMS_CLAIM] where Claim_Id in (@view,@add,@edit,@delete)