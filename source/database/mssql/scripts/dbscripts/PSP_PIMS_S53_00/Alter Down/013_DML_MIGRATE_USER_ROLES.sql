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
DECLARE @admin BIGINT;
SELECT @finance = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Finance';
SELECT @functional = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Functional';
SELECT @restricted = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Functional (Restricted)';
SELECT @readOnly = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Read Only';
SELECT @admin = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'System Administrator';

-- new roles
DECLARE @acqfunc BIGINT;
DECLARE @acgrdon BIGINT;
DECLARE @llfunc  BIGINT;
DECLARE @llrdon  BIGINT;
DECLARE @prjfunc BIGINT;
DECLARE @prjrdon BIGINT;
DECLARE @resfunc BIGINT;
DECLARE @resrdon BIGINT;
DECLARE @sysadmn BIGINT;
--
SELECT @acqfunc = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Acquisition functional';
SELECT @acgrdon = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Acquisition read-only';
SELECT @llfunc  = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Lease/License functional';
SELECT @llrdon  = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Lease/License read-only';
SELECT @prjfunc = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Project functional';
SELECT @prjrdon = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Project read-only';
SELECT @resfunc = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Research functional';
SELECT @resrdon = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Research read-only';
SELECT @sysadmn = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'System administrator';

INSERT INTO PIMS_USER_ROLE (ROLE_ID, USER_ID, APP_CREATE_USERID, APP_LAST_UPDATE_USERID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_USER_DIRECTORY) 
SELECT DISTINCT @functional, USER_ID, 'db-migration', 'db-migration', 'db-migration', 'db-migration'
FROM PIMS_USER_ROLE
WHERE ROLE_ID in (@acqfunc, @llfunc, @prjfunc, @resfunc)

INSERT INTO PIMS_USER_ROLE (ROLE_ID, USER_ID, APP_CREATE_USERID, APP_LAST_UPDATE_USERID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_USER_DIRECTORY) 
SELECT DISTINCT @restricted, USER_ID, 'db-migration', 'db-migration', 'db-migration', 'db-migration'
FROM PIMS_USER_ROLE
WHERE ROLE_ID in (@acgrdon, @llrdon, @prjrdon, @resrdon)

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
