UPDATE PIMS_ROLE SET IS_DISABLED = 1, CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 WHERE NAME IN ('Organization Administrator', 'Undetermined', 'Real Estate Manager', 'Real Estate Analyst')
GO

INSERT INTO PIMS_ROLE (ROLE_UID, NAME, DESCRIPTION, IS_PUBLIC, IS_DISABLED, SORT_ORDER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_DIRECTORY)
VALUES
  (NEWID(), N'Finance', N'Finance team members.', CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Functional', N'PLMB staff (includes team members from HQ, regions and districts).', CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Functional (Restricted)', N'Contractors, Internal ministry staff.', CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Read Only', N'Other ministries (e.g. Attorney General).', CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data');
GO

DECLARE @functional BIGINT;
DECLARE @readOnly BIGINT;
DECLARE @rem BIGINT;
DECLARE @rea BIGINT;
SELECT @functional = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Functional';
SELECT @readOnly = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Read Only';
SELECT @rem = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Real Estate Manager';
SELECT @rea = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Real Estate Analyst';

UPDATE PIMS_USER_ROLE SET ROLE_ID = @functional, CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 WHERE ROLE_ID = @rea;
UPDATE PIMS_USER_ROLE SET ROLE_ID = @readOnly, CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 WHERE ROLE_ID = @rem;
GO