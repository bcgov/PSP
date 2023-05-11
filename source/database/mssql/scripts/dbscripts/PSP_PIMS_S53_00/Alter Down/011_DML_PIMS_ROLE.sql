-- Re-add old PIMS roles removed in IS53 in preparation for migration.
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

INSERT INTO PIMS_ROLE (ROLE_UID, NAME, DESCRIPTION, IS_PUBLIC, IS_DISABLED, SORT_ORDER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_DIRECTORY)
VALUES
  (NEWID(), N'Finance', N'Finance team members.', CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Functional', N'PLMB staff (includes team members from HQ, regions and districts).', CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Functional (Restricted)', N'Contractors, Internal ministry staff.', CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Read Only', N'Other ministries (e.g. Attorney General).', CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data');

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
