SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- old roles
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

-- DELETE THE NEW ROLES' CLAIMS AND ALL NEW ROLES
DELETE FROM PIMS_ACCESS_REQUEST WHERE ROLE_ID IN (@acqfunc, @acgrdon, @llfunc, @llrdon, @prjfunc, @prjrdon, @resfunc, @resrdon)
DELETE FROM PIMS_ROLE_CLAIM WHERE ROLE_ID IN (@acqfunc, @acgrdon, @llfunc, @llrdon, @prjfunc, @prjrdon, @resfunc, @resrdon)
DELETE FROM PIMS_USER_ROLE WHERE ROLE_ID IN (@acqfunc, @acgrdon, @llfunc, @llrdon, @prjfunc, @prjrdon, @resfunc, @resrdon)
DELETE FROM PIMS_ROLE WHERE ROLE_ID IN (@acqfunc, @acgrdon, @llfunc, @llrdon, @prjfunc, @prjrdon, @resfunc, @resrdon)

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
