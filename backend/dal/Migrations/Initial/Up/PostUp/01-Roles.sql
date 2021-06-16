PRINT 'Adding Roles'
INSERT INTO
    dbo.[Roles] (
        [Id],
        [Name],
        [Description],
        [IsPublic],
        [IsDisabled],
        [SortOrder]
    )
VALUES
    (
        'bbf27108-a0dc-4782-8025-7af7af711335',
        'System Administrator',
        'System Administrator of the PIMS solution.',
        0,
        0,
        0
    ),
    (
        '6ae8448d-5f0a-4607-803a-df0bc4efdc0f',
        'Agency Administrator',
        'Agency Administrator of the users agency.',
        0,
        0,
        0
    ),
    (
        'aad8c03d-892c-4cc3-b992-5b41c4f2392c',
        'Real Estate Manager',
        'Real Estate Manager can manage properties within their agencies.',
        1,
        0,
        0
    ),
    (
        '7a7b2549-ae85-4ad6-a8d3-3a5f8d4f9ca5',
        'Real Estate Analyst',
        'Real Estate Analyst can manage properties within their agencies.',
        1,
        0,
        0
    ),
    (
        '08c52eec-6917-4512-ac02-7d7ff89ed7a6',
        'SRES',
        'SRES group provides a way to add additional claims to users (i.e. property-delete).',
        0,
        0,
        0
    ),
    (
        'd416f362-1e6f-4e24-a561-c6bb45a35194',
        'SRES Financial Manager',
        'SRES Financial Manager has claims specific to administering and viewing reports of financial data.',
        0,
        0,
        0
    ),
    (
        '5c6cea5b-9b7c-47e8-852c-693e90ed815e',
        'SRES Financial Reporter',
        'The SRES Financial Reporter can view, create, and delete non-final SPL reports.',
        0,
        0,
        0
    );
