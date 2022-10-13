DECLARE @generateDocuments BIGINT;
SELECT @generateDocuments = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'generate-documents';

DELETE FROM [dbo].[PIMS_CLAIM] WHERE CLAIM_ID = @generateDocuments;
GO
