-- assign claims to all new roles.

DECLARE @appUserGuid uniqueidentifier = NEWID();

-- ****************************************************************************
-- Declare and initialize the roles
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

-- ****************************************************************************
-- Declare and initialize the claims
-- ****************************************************************************
DECLARE @leaseView   BIGINT;
DECLARE @leaseAdd    BIGINT;
DECLARE @leaseEdit   BIGINT;
DECLARE @leaseDelete BIGINT;
--
select @leaseView   = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'lease-view';
select @leaseAdd    = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'lease-add';
select @leaseEdit   = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'lease-edit';
select @leaseDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'lease-delete';
-- ----------------------------------------------------------------------------
DECLARE @researchView   BIGINT;
DECLARE @researchAdd    BIGINT;
DECLARE @researchEdit   BIGINT;
DECLARE @researchDelete BIGINT;
--
select @researchView   = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'researchfile-view';
select @researchAdd    = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'researchfile-add';
select @researchEdit   = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'researchfile-edit';
select @researchDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'researchfile-delete';
-- ----------------------------------------------------------------------------
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

DECLARE @rolePimsR BIGINT;
select @rolePimsR = CLAIM_ID FROM PIMS_CLAIM where NAME = 'ROLE_PIMS_R';

DECLARE @adminUsers BIGINT;
DECLARE @adminProjects BIGINT;
DECLARE @adminProperties BIGINT;
DECLARE @systemAdministrator BIGINT;
select @adminUsers = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'admin-users';
select @adminProjects = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'admin-projects';
select @adminProperties = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'admin-properties';
select @systemAdministrator = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'system-administrator';

DECLARE @acquisitionView BIGINT;
DECLARE @acquisitionAdd BIGINT;
DECLARE @acquisitionEdit BIGINT;
DECLARE @acquisitionDelete BIGINT;
select @acquisitionView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'acquisitionfile-view';
select @acquisitionAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'acquisitionfile-add';
select @acquisitionEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'acquisitionfile-edit';
select @acquisitionDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'acquisitionfile-delete';

DECLARE @noteView BIGINT;
DECLARE @noteAdd BIGINT;
DECLARE @noteEdit BIGINT;
DECLARE @noteDelete BIGINT;
SELECT @noteView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'note-view';
SELECT @noteAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'note-add';
SELECT @noteEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'note-edit';
SELECT @noteDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'note-delete';

DECLARE @documentView BIGINT;
DECLARE @documentAdd BIGINT;
DECLARE @documentEdit BIGINT;
DECLARE @documentDelete BIGINT;
DECLARE @documentAdmin BIGINT;
SELECT @documentView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'document-view';
SELECT @documentAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'document-add';
SELECT @documentEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'document-edit';
SELECT @documentDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'document-delete';
SELECT @documentAdmin = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'document-admin';

DECLARE @activityView BIGINT;
DECLARE @activityAdd BIGINT;
DECLARE @activityEdit BIGINT;
DECLARE @activityDelete BIGINT;
SELECT @activityView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-view';
SELECT @activityAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-add';
SELECT @activityEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-edit';
SELECT @activityDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'activity-delete';

DECLARE @projectView BIGINT;
DECLARE @projectAdd BIGINT;
DECLARE @projectEdit BIGINT;
DECLARE @projectDelete BIGINT;
SELECT @projectView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'project-view';
SELECT @projectAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'project-add';
SELECT @projectEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'project-edit';
SELECT @projectDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'project-delete';

DECLARE @formView BIGINT;
DECLARE @formAdd BIGINT;
DECLARE @formEdit BIGINT;
DECLARE @formDelete BIGINT;
SELECT @formView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'form-view';
SELECT @formAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'form-add';
SELECT @formEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'form-edit';
SELECT @formDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'form-delete';

DECLARE @agreementView BIGINT;
SELECT @agreementView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'agreement-view';

DECLARE @compensationView BIGINT;
DECLARE @compensationAdd BIGINT;
DECLARE @compensationEdit BIGINT;
DECLARE @compensationDelete BIGINT;
SELECT @compensationView = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'compensation-requisition-view';
SELECT @compensationAdd = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'compensation-requisition-add';
SELECT @compensationEdit = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'compensation-requisition-edit';
SELECT @compensationDelete = CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'compensation-requisition-delete';

INSERT INTO [dbo].[PIMS_ROLE_CLAIM] ([ROLE_ID], [CLAIM_ID], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [APP_LAST_UPDATE_USER_DIRECTORY])
VALUES
    -- Acquisition Functional
    (@acqfunc, @propertyView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @propertyAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @propertyEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @propertyDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @contactView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @contactAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @contactEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @contactDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @rolePimsR,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @noteView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @noteAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @noteEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @noteDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @documentView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @documentAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @documentEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @documentDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @formView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @formAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @formEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @formDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @acquisitionView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @acquisitionAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @acquisitionEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @acquisitionDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @compensationView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @compensationAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @compensationEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @compensationDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @projectView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acqfunc, @agreementView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    -- Acquisition Read
    (@acgrdon, @propertyView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acgrdon, @contactView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acgrdon, @rolePimsR,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acgrdon, @noteView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acgrdon, @documentView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acgrdon, @projectView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acgrdon, @agreementView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@acgrdon, @acquisitionView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    -- Lease Functional
    (@llfunc, @propertyView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @propertyAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @propertyEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @propertyDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @contactView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @contactAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @contactEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @contactDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @rolePimsR,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @noteView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @noteAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @noteEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @noteDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @documentView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @documentAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @documentEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @documentDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @formView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @formAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @formEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @formDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @leaseView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @leaseAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @leaseEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llfunc, @leaseDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    -- Lease Read
    (@llrdon, @propertyView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llrdon, @contactView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llrdon, @rolePimsR,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llrdon, @noteView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llrdon, @documentView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llrdon, @projectView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@llrdon, @leaseView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    -- Project Functional
    (@prjfunc, @projectView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@prjfunc, @projectAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@prjfunc, @projectEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@prjfunc, @projectDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@prjfunc, @propertyView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    -- Project Read
    (@prjrdon, @projectView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@prjrdon, @propertyView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    -- Research Functional
    (@resfunc, @propertyView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @propertyAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @propertyEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @propertyDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @contactView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @contactAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @contactEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @contactDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @rolePimsR,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @noteView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @noteAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @noteEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @noteDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @documentView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @documentAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @documentEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @documentDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @formView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @formAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @formEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @formDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @researchView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @researchAdd,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @researchEdit,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resfunc, @researchDelete,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    -- Research Read
    (@resrdon, @propertyView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resrdon, @contactView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resrdon, @rolePimsR,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resrdon, @noteView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resrdon, @documentView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resrdon, @projectView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', ''),
    (@resrdon, @researchView,                    N'SEED', @appUserGuid, N'SEED', @appUserGuid, '', '')
GO
