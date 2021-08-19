PRINT N'Adding [PIMS_ROLE_CLAIM]'

INSERT INTO dbo.[PIMS_ROLE_CLAIM] (
    [ROLE_ID]
    , [CLAIM_ID]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
)
VALUES
-- system-administrator
(
    1
    , 8   -- property-view
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    1
    , 9   -- property-add
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    1
    , 10   -- property-edit
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    1
    , 11   -- property-delete
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    1
    , 12   -- sensitive-view
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    1
    , 3   -- admin-users
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    1
    , 4   -- admin-roles
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    1
    , 5   -- admin-properties
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    1
    , 6   -- admin-organizations
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    1
    , 7   -- admin-projects
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    1
    , 1   -- system-administrator
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    1
    , 2   -- organization-administrator
    , 'migration'
    , ''
    , 'migration'
    , ''
),


-- organization-administrator
(
    2
    , 8   -- property-view
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    2
    , 9   -- property-add
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    2
    , 10   -- property-edit
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    2
    , 12   -- sensitive-view
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    2
    , 3   -- admin-users
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    2
    , 2   -- organization-administrator
    , 'migration'
    , ''
    , 'migration'
    , ''
),

-- real-estate-manager
(
    3
    , 8   -- property-view
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    3
    , 9   -- property-add
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    3
    , 10   -- property-edit
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    3
    , 12   -- sensitive-view
    , 'migration'
    , ''
    , 'migration'
    , ''
),

-- real-estate-assistant
(
    4
    , 8   -- property-view
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    4
    , 9   -- property-add
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    4
    , 10   -- property-edit
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    4
    , 12   -- sensitive-view
    , 'migration'
    , ''
    , 'migration'
    , ''
)
