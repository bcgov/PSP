-- clean up deprecated roles, insert new roles to prepare for migration from old roles to new roles.
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- clean up old deprecated roles
DECLARE @orgAdmin BIGINT;
DECLARE @realEstateManager BIGINT;
DECLARE @realEstateAnalyst BIGINT;
SELECT @orgAdmin = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Organization Administrator';
SELECT @realEstateManager = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Real Estate Manager';
SELECT @realEstateAnalyst = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Real Estate Analyst';
DELETE FROM PIMS_ACCESS_REQUEST WHERE ROLE_ID IN (@orgAdmin, @realEstateManager, @realEstateAnalyst)
DELETE FROM PIMS_USER_ROLE WHERE ROLE_ID IN (@orgAdmin, @realEstateManager, @realEstateAnalyst)
DELETE FROM PIMS_ROLE_CLAIM WHERE ROLE_ID IN (@orgAdmin, @realEstateManager, @realEstateAnalyst)
DELETE FROM PIMS_ROLE WHERE ROLE_ID = @orgAdmin;
DELETE FROM PIMS_ROLE WHERE ROLE_ID = @realEstateManager;
DELETE FROM PIMS_ROLE WHERE ROLE_ID = @realEstateAnalyst;

-- Insert the new roles
PRINT N'Insert the new roles'
GO
INSERT INTO PIMS_ROLE (ROLE_UID, NAME, DESCRIPTION, SORT_ORDER, IS_PUBLIC, APP_CREATE_USERID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_DIRECTORY)
VALUES
  (NEWID(), N'Acquisition functional',   N'Access to create, read, update Acquisition files.',     CONVERT([bit],(0)), CONVERT([bit],(1)), N'Seed Data', N'Seed Data', N'Seed Data', N'Seed Data'),
  (NEWID(), N'Acquisition read-only',    N'Access to read Acquisition files.',                     CONVERT([bit],(0)), CONVERT([bit],(1)), N'Seed Data', N'Seed Data', N'Seed Data', N'Seed Data'),
  (NEWID(), N'Lease/License functional', N'Access to create, read, update Leases/Licenses files.', CONVERT([bit],(0)), CONVERT([bit],(1)), N'Seed Data', N'Seed Data', N'Seed Data', N'Seed Data'),
  (NEWID(), N'Lease/License read-only',  N'Access to read lease/license.',                         CONVERT([bit],(0)), CONVERT([bit],(1)), N'Seed Data', N'Seed Data', N'Seed Data', N'Seed Data'),
  (NEWID(), N'Project functional',       N'Access to create, read, update projects.',              CONVERT([bit],(0)), CONVERT([bit],(1)), N'Seed Data', N'Seed Data', N'Seed Data', N'Seed Data'),
  (NEWID(), N'Project read-only',        N'Access to read Projects.',                              CONVERT([bit],(0)), CONVERT([bit],(1)), N'Seed Data', N'Seed Data', N'Seed Data', N'Seed Data'),
  (NEWID(), N'Research functional',      N'Access to create, read, update Research files.',        CONVERT([bit],(0)), CONVERT([bit],(1)), N'Seed Data', N'Seed Data', N'Seed Data', N'Seed Data'),
  (NEWID(), N'Research read-only',       N'Access to read Research files.',                        CONVERT([bit],(0)), CONVERT([bit],(1)), N'Seed Data', N'Seed Data', N'Seed Data', N'Seed Data');
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

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
