DECLARE @functional BIGINT;
SELECT @functional = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Functional';

DECLARE @leaseDelete BIGINT;
select @leaseDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'lease-delete';

/* Functional */
DELETE FROM [dbo].[PIMS_ROLE_CLAIM] WHERE CLAIM_ID = @leaseDelete AND ROLE_ID = @functional;

GO