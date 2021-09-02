PRINT N'Adding [PIMS_CLAIM]'
INSERT INTO dbo.[PIMS_CLAIM] (
    [CLAIM_ID]
    , [CLAIM_UID]
    , [NAME]
    , [DESCRIPTION]
    , [IS_DISABLED]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
)
VALUES
-- Administration
(
    1
    , 'b9e1e966-d2aa-420f-83c4-617b984d1268'
    , 'system-administrator'
    , 'Ability to administrate system.'
    , 0
    , 'system'
    , ''
    , 'system'
    , ''
), (
    2
    , '6efd16d4-41ca-4feb-86f5-7598691f7bc6'
    , 'organization-administrator'
    , 'Ability to administrate organizations.'
    , 0
    , 'system'
    , ''
    , 'system'
    , ''
),
(
    3
    , 'fd86ddec-8f9d-4d7b-8c69-956062c5104f'
    , 'admin-users'
    , 'Ability to administrate users.'
    , 0
    , 'system'
    , ''
    , 'system'
    , ''
), (
    4
    , '321e245b-ee7d-4d7c-83a8-56b5a9d33c2d'
    , 'admin-roles'
    , 'Ability to administrate roles.'
    , 0
    , 'system'
    , ''
    , 'system'
    , ''
), (
    5
    , '71e74513-a036-4df3-b724-a8c349b7fc28'
    , 'admin-properties'
    , 'Ability to administrate properties.'
    , 0
    , 'system'
    , ''
    , 'system'
    , ''
), (
    6
    , '9b556b3f-441f-4d11-9f6f-14d455df4e05'
    , 'admin-organizations'
    , 'Ability to administrate organizations.'
    , 0
    , 'system'
    , ''
    , 'system'
    , ''
), (
    7
    , 'c46ccf94-4b3c-486f-b34c-9a707a54f357'
    , 'admin-projects'
    , 'Ability to administrate projects.'
    , 0
    , 'system'
    , ''
    , 'system'
    , ''
),

-- Properties
(
    8
    , '91fc8939-2dea-44a1-bd17-a1c8f0fe5dc1'
    , 'property-view'
    , 'Ability to view properties.'
    , 0
    , 'system'
    , ''
    , 'system'
    , ''
), (
    9
    , '5fd96f19-abe1-47e7-8a54-0a707bc3e4a4'
    , 'property-add'
    , 'Ability to add properties.'
    , 0
    , 'system'
    , ''
    , 'system'
    , ''
), (
    10
    , '223664c7-650c-40ac-8581-f40e10064537'
    , 'property-edit'
    , 'Ability to edit properties.'
    , 0
    , 'system'
    , ''
    , 'system'
    , ''
), (
    11
    , '223664c7-650c-40ac-8581-f40e10164537'
    , 'property-delete'
    , 'Ability to delete properties.'
    , 0
    , 'system'
    , ''
    , 'system'
    , ''
), (
    12
    , '4dc0f39a-32f0-43a4-9d90-62fd94f20567'
    , 'sensitive-view'
    , 'Ability to view sensitive properties.'
    , 0
    , 'system'
    , ''
    , 'system'
    , ''
)

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_CLAIM_ID_SEQ]
RESTART WITH 13

