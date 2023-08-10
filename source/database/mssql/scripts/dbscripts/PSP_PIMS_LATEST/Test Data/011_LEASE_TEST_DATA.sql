-- Add pids for lease and license header
UPDATE
    [dbo].[PIMS_PROPERTY_LEASE]
SET
    [LEASE_ID] = 2,
    [CONCURRENCY_CONTROL_NUMBER] = [CONCURRENCY_CONTROL_NUMBER] + 1
WHERE
    [LEASE_ID] = 45

-- Add improvements for lease
INSERT INTO
    dbo.[PIMS_PROPERTY_IMPROVEMENT]
    (
    [LEASE_ID],
    [PROPERTY_IMPROVEMENT_TYPE_CODE],
    [IMPROVEMENT_DESCRIPTION],
    [STRUCTURE_SIZE],
    [CONCURRENCY_CONTROL_NUMBER]
    )
VALUES
    (
        2,
        'COMMBLDG',
        'This is a test description for the purpose of testing things and ensuring they are testable. ',
        '1234 sq ft.',
        1
    ),
    (
        2,
        'OTHER',
        'This is a test description for the purpose of testing things and ensuring they are testable. ',
        '111 sq ft.',
        1
    ),
    (
        2,
        'RTA',
        'This is a test description for the purpose of testing things and ensuring they are testable. ',
        '222 sq ft.',
        1
    )
-- Associate property with lease
UPDATE
    [dbo].[PIMS_PROPERTY_LEASE]
SET
    [LEASE_ID] = 2,
    [CONCURRENCY_CONTROL_NUMBER] = [CONCURRENCY_CONTROL_NUMBER] + 1
WHERE
    [LEASE_ID] = 17

-- Populate random l file number for leases
UPDATE
    [dbo].[PIMS_LEASE]
SET
    [L_FILE_NO] = CAST(
        LEFT(
            CAST(
                ABS(CAST(CAST(NEWID() as BINARY(10)) as int)) as varchar(max)
            ) + '00000000',
            9
        ) as int
    ),
    [CONCURRENCY_CONTROL_NUMBER] = [CONCURRENCY_CONTROL_NUMBER] + 1
WHERE
    [L_FILE_NO] IS NULL

-- Populate tenant name for leases
UPDATE
    [dbo].[PIMS_LEASE_TENANT]
SET
    [PERSON_ID] = 4,
    [CONCURRENCY_CONTROL_NUMBER] = [CONCURRENCY_CONTROL_NUMBER] + 1
WHERE
    [PERSON_ID] IS NULL

-- Populate different tenant name on even id's for varience
UPDATE
    [dbo].[PIMS_LEASE_TENANT]
SET
    [PERSON_ID] = 1,
    [CONCURRENCY_CONTROL_NUMBER] = [CONCURRENCY_CONTROL_NUMBER] + 1
WHERE
    [LEASE_TENANT_ID] % 2 = 0
UPDATE
    [dbo].[PIMS_LEASE_TENANT]
SET
    [PERSON_ID] = 2,
    [CONCURRENCY_CONTROL_NUMBER] = [CONCURRENCY_CONTROL_NUMBER] + 1
WHERE
    [PERSON_ID] IS NULL

-- Make sure every lease has property associated to it (331 seed property leases, and 110 leases)
INSERT INTO
    [dbo].[PIMS_PROPERTY_LEASE]
    (
    [PROPERTY_ID],
    [LEASE_ID],
    [CONCURRENCY_CONTROL_NUMBER]
    )
VALUES
    (1, 1, 1),
    (2, 2, 1),
    (3, 3, 1),
    (4, 4, 1),
    (5, 5, 1),
    (6, 6, 1),
    (7, 7, 1),
    (8, 8, 1),
    (9, 9, 1),
    (10, 10, 1),
    (11, 11, 1),
    (12, 12, 1),
    (13, 13, 1),
    (14, 14, 1),
    (15, 15, 1),
    (16, 16, 1),
    (17, 17, 1),
    (18, 18, 1),
    (19, 19, 1),
    (20, 20, 1),
    (21, 21, 1),
    (22, 22, 1),
    (23, 23, 1),
    (24, 24, 1),
    (25, 25, 1),
    (26, 26, 1),
    (27, 27, 1),
    (28, 28, 1),
    (29, 29, 1),
    (30, 30, 1),
    (31, 31, 1),
    (32, 32, 1),
    (33, 33, 1),
    (34, 34, 1),
    (35, 35, 1),
    (36, 36, 1),
    (37, 37, 1),
    (38, 38, 1),
    (39, 39, 1),
    (40, 40, 1),
    (41, 41, 1),
    (42, 42, 1),
    (43, 43, 1),
    (44, 44, 1),
    (45, 45, 1),
    (46, 46, 1),
    (47, 47, 1),
    (48, 48, 1),
    (49, 49, 1),
    (50, 50, 1),
    (51, 51, 1),
    (52, 52, 1),
    (53, 53, 1),
    (54, 54, 1),
    (55, 55, 1)
