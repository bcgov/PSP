DECLARE @managementFunctional BIGINT;
DECLARE @managementReadOnly BIGINT;
SELECT @managementFunctional = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Management functional';
SELECT @managementReadOnly = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Management read-only';


DELETE FROM [dbo].[PIMS_ROLE] WHERE ROLE_ID = @managementFunctional;
DELETE FROM [dbo].[PIMS_ROLE] WHERE ROLE_ID = @managementReadOnly;

GO
