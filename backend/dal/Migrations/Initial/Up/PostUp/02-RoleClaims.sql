PRINT N'Adding [PIMS_ROLE_CLAIM]'

INSERT INTO dbo.[PIMS_ROLE_CLAIM]
    (
    [ROLE_ID]
    , [CLAIM_ID]
    )
VALUES
-- system-administrator
(
    1
    , 8   -- property-view
),
(
    1
    , 9   -- property-add
),
(
    1
    , 10   -- property-edit
),
(
    1
    , 11   -- property-delete
),
(
    1
    , 12   -- sensitive-view
),
(
    1
    , 3   -- admin-users
),
(
    1
    , 4   -- admin-roles
),
(
    1
    , 5   -- admin-properties
),
(
    1
    , 6   -- admin-agencies
),
(
    1
    , 7   -- admin-projects
),
(
    1
    , 15   -- project-view
),
(
    1
    , 16   -- project-add
),
(
    1
    , 17   -- project-edit
),
(
    1
    , 18   -- project-delete
),
(
    1
    , 1   -- system-administrator
),
(
    1
    , 2   -- agency-administrator
),


-- agency-administrator
(
    2
    , 8   -- property-view
),
(
    2
    , 9   -- property-add
),
(
    2
    , 10   -- property-edit
),
(
    2
    , 12   -- sensitive-view
),
(
    2
    , 3   -- admin-users
),
(
    2
    , 15   -- project-view
),
(
    2
    , 16   -- project-add
),
(
    2
    , 17   -- project-edit
),
(
    2
    , 18   -- project-delete
),
(
    2
    , 2   -- agency-administrator
),

-- real-estate-manager
(
    3
    , 8   -- property-view
),
(
    3
    , 9   -- property-add
),
(
    3
    , 10   -- property-edit
),
(
    3
    , 13   -- dispose-request
),
(
    3
    , 12   -- sensitive-view
),
(
    3
    , 15   -- project-view
),
(
    3
    , 16   -- project-add
),
(
    3
    , 17   -- project-edit
),
(
    3
    , 18   -- project-delete
),

-- real-estate-assistant
(
    4
    , 8   -- property-view
),
(
    4
    , 9   -- property-add
),
(
    4
    , 10   -- property-edit
),
(
    4
    , 12   -- sensitive-view
),
(
    4
    , 15   -- project-view
),
(
    4
    , 16   -- project-add
),
(
    4
    , 17   -- project-edit
)
