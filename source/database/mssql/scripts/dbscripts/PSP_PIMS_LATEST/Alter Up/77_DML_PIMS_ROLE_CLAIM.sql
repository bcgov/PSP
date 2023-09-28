BEGIN TRANSACTION;

DECLARE @dbUserId nvarchar(30) = 'system';
DECLARE @dbUserGuid uniqueidentifier = null;
DECLARE @appUserId nvarchar(30) = N'SEED';
DECLARE @appUserGuid uniqueidentifier = '1b93f614-91da-4b32-b36e-bd2c6ebd12e2';
DECLARE @appUserDirectory nvarchar(30) = N'';
DECLARE @seedTime DateTime = GETDATE();

DECLARE @functional BIGINT;
DECLARE @readOnly BIGINT;
DECLARE @systemAdministrator BIGINT;
SELECT @functional = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Management functional';
SELECT @readOnly = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Management read-only';
SELECT @systemAdministrator = ROLE_ID FROM PIMS_ROLE WHERE NAME = 'System Administrator';

-- ****************************************************************************
-- Declare and initialize the claims
-- ****************************************************************************
DECLARE @propertyView   BIGINT;
DECLARE @propertyAdd    BIGINT;
DECLARE @propertyEdit   BIGINT;
DECLARE @propertyDelete BIGINT;
--
select @propertyView   = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'property-view';
select @propertyAdd    = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'property-add';
select @propertyEdit   = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'property-edit';
select @propertyDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'property-delete';
-- ----------------------------------------------------------------------------
DECLARE @contactView   BIGINT;
DECLARE @contactAdd    BIGINT;
DECLARE @contactEdit   BIGINT;
DECLARE @contactDelete BIGINT;
--
select @contactView   = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'contact-view';
select @contactAdd    = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'contact-add';
select @contactEdit   = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'contact-edit';
select @contactDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'contact-delete';
-- ----------------------------------------------------------------------------
DECLARE @managementView BIGINT;
DECLARE @managementAdd BIGINT;
DECLARE @managementEdit BIGINT;
DECLARE @managementDelete BIGINT;
--
SELECT @managementView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'management-view';
SELECT @managementAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'management-add';
SELECT @managementEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'management-edit';
SELECT @managementDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'management-delete';
-- ----------------------------------------------------------------------------
DECLARE @noteView BIGINT;
DECLARE @noteAdd BIGINT;
DECLARE @noteEdit BIGINT;
DECLARE @noteDelete BIGINT;
--
SELECT @noteView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'note-view';
SELECT @noteAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'note-add';
SELECT @noteEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'note-edit';
SELECT @noteDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'note-delete';
-- ----------------------------------------------------------------------------
DECLARE @documentView BIGINT;
DECLARE @documentAdd BIGINT;
DECLARE @documentEdit BIGINT;
DECLARE @documentDelete BIGINT;
--
SELECT @documentView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'document-view';
SELECT @documentAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'document-add';
SELECT @documentEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'document-edit';
SELECT @documentDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'document-delete';
-- ----------------------------------------------------------------------------
DECLARE @rolePimsR BIGINT;
--
select @rolePimsR = CLAIM_ID FROM PIMS_CLAIM where NAME = 'ROLE_PIMS_R';

/* Management Functional */
INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @managementView, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @managementEdit, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @managementAdd, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @managementDelete, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)


INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @propertyView, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @propertyEdit, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @propertyAdd, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @propertyDelete, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)


INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @contactView, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @contactEdit, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @contactAdd, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @contactDelete, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)


INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @noteView, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @noteEdit, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @noteAdd, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @noteDelete, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)


INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @documentView, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @documentEdit, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @documentAdd, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @documentDelete, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)


INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@functional, @rolePimsR, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

/* System Administrator */
INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@systemAdministrator, @managementView, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@systemAdministrator, @managementEdit, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@systemAdministrator, @managementAdd, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@systemAdministrator, @managementDelete, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)

/* Management Read Only */
INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@readOnly, @managementView, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)


INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@readOnly, @propertyView, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)


INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@readOnly, @contactView, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)


INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@readOnly, @noteView, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)


INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@readOnly, @documentView, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)


INSERT [dbo].[PIMS_ROLE_CLAIM]
    ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID])
VALUES
    (@readOnly, @rolePimsR, 0, 1, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId, @seedTime, @appUserId, @appUserGuid, @appUserDirectory, @seedTime, @dbUserId)


COMMIT TRANSACTION;
