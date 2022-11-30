DECLARE @devinPersonId bigint;

DECLARE @alejandroPersonId bigint;

DECLARE @manuelPersonId bigint;

DECLARE @maheshPersonId bigint;

INSERT INTO
    [dbo].[PIMS_PERSON] (
        [SURNAME],
        [FIRST_NAME],
        [APP_CREATE_USERID],
        [APP_CREATE_USER_DIRECTORY],
        [APP_LAST_UPDATE_USERID],
        [APP_LAST_UPDATE_USER_DIRECTORY],
        [DB_CREATE_USERID],
        [DB_LAST_UPDATE_USERID]
    )
VALUES
    (
        'Smith',
        'Devin',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        'Sanchez',
        'Alejandro',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        'Rodriguez',
        'Manuel',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        'Babu',
        'Mahesh',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    );

SET
    @devinPersonId = (
        SELECT
            TOP(1) PERSON_ID
        FROM
            PIMS_PERSON
        WHERE
            SURNAME = 'Smith'
    );

SET
    @alejandroPersonId = (
        SELECT
            TOP(1) PERSON_ID
        FROM
            PIMS_PERSON
        WHERE
            SURNAME = 'Sanchez'
    );

SET
    @manuelPersonId = (
        SELECT
            TOP(1) PERSON_ID
        FROM
            PIMS_PERSON
        WHERE
            SURNAME = 'Rodriguez'
    );

SET
    @maheshPersonId = (
        SELECT
            TOP(1) PERSON_ID
        FROM
            PIMS_PERSON
        WHERE
            SURNAME = 'Babu'
    );

INSERT INTO
    [dbo].[PIMS_USER] (
        [PERSON_ID],
        [BUSINESS_IDENTIFIER_VALUE],
        [GUID_IDENTIFIER_VALUE],
        [IS_DISABLED],
        [ISSUE_DATE],
        [APP_CREATE_USERID],
        [APP_CREATE_USER_DIRECTORY],
        [APP_LAST_UPDATE_USERID],
        [APP_LAST_UPDATE_USER_DIRECTORY],
        [DB_CREATE_USERID],
        [DB_LAST_UPDATE_USERID]
    )
VALUES
    (
        @devinPersonId,
        'desmith',
        '7DB28007-0D47-4EF0-BB46-C365A4B95A73',
        0,
        CURRENT_TIMESTAMP,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @alejandroPersonId,
        'asanchez',
        '4109E6B4-585C-4678-8A24-1A99B45E3A5D',
        0,
        CURRENT_TIMESTAMP,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @manuelPersonId,
        'mrodriguez',
        '4109E6B4-585C-4678-8A24-1A99B45E3A5D',
        0,
        CURRENT_TIMESTAMP,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @maheshPersonId,
        'mbabu',
        '5D661D1E-14F0-477C-A7FB-31F21E8B4FDA',
        0,
        CURRENT_TIMESTAMP,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    );

GO