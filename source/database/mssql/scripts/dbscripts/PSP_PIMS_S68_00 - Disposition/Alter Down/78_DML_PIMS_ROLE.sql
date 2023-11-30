DECLARE @dispositionFunctional BIGINT;
DECLARE @dispositionReadOnly BIGINT;
SELECT @dispositionFunctional = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Disposition functional';
SELECT @dispositionReadOnly = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Disposition read-only';


DELETE FROM [dbo].[PIMS_ROLE] WHERE ROLE_ID = @dispositionFunctional;
DELETE FROM [dbo].[PIMS_ROLE] WHERE ROLE_ID = @dispositionReadOnly;

GO
