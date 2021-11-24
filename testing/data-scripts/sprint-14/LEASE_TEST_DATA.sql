-- Add pids for lease and license header
UPDATE [PIMS_TST].[dbo].[PIMS_PROPERTY_LEASE]
SET [LEASE_ID] = 2, [CONCURRENCY_CONTROL_NUMBER] = 2
WHERE [LEASE_ID] = 45

-- Add improvements for lease
INSERT INTO dbo.[PIMS_PROPERTY_IMPROVEMENT]
    (
    [PROPERTY_LEASE_ID]
    ,[PROPERTY_IMPROVEMENT_TYPE_CODE]
    ,[IMPROVEMENT_DESCRIPTION]
    ,[STRUCTURE_SIZE]
    ,[UNIT]
    ,[CONCURRENCY_CONTROL_NUMBER]
    )
VALUES
    (
        2
    , 'COMMBLDG'
    , 'This is a test description for the purpose of testing things and ensuring they are testable. '
    , '1234'
    , '211 (2nd floor)'
	, 1
),
    (
        2
    , 'OTHER'
    , 'This is a test description for the purpose of testing things and ensuring they are testable. '
    , '111'
    , '311 (Upper unit)'
	, 1
),
    (
        2
    , 'RTA'
    , 'This is a test description for the purpose of testing things and ensuring they are testable. '
    , '222 sq ft'
    , '111 (Lower unit)'
	, 1
)

-- Associate property with lease
UPDATE [PIMS_TST].[dbo].[PIMS_PROPERTY_LEASE]
SET [LEASE_ID] = 2, [CONCURRENCY_CONTROL_NUMBER] = 2
WHERE [LEASE_ID] = 45
