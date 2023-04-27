DECLARE @agreementView BIGINT;
SELECT @agreementView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'agreement-view';


DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @agreementView;

GO