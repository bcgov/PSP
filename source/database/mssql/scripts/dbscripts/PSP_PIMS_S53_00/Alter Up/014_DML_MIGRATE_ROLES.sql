-- delete all related role entities and then then the old roles themselves.
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- old roles
DECLARE @finance BIGINT;
DECLARE @functional BIGINT;
DECLARE @restricted BIGINT;
DECLARE @readOnly BIGINT;
SELECT @finance = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Finance';
SELECT @functional = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Functional';
SELECT @restricted = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Functional (Restricted)';
SELECT @readOnly = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Read Only';

DELETE FROM PIMS_ACCESS_REQUEST WHERE ROLE_ID IN (@restricted, @functional, @readOnly, @finance)
DELETE FROM PIMS_USER_ROLE WHERE ROLE_ID IN (@restricted, @functional, @readOnly, @finance)
DELETE FROM PIMS_ROLE_CLAIM WHERE ROLE_ID IN (@restricted, @functional, @readOnly, @finance)
DELETE FROM PIMS_ROLE WHERE ROLE_ID IN (@restricted, @functional, @readOnly, @finance)

COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
   IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
   PRINT 'The database update failed'
END
GO
