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
            SURNAME = 'Smith' AND FIRST_NAME = 'Devin'
    );

SET
    @alejandroPersonId = (
        SELECT
            TOP(1) PERSON_ID
        FROM
            PIMS_PERSON
        WHERE
            SURNAME = 'Sanchez' AND FIRST_NAME = 'Alejandro'
    );

SET
    @manuelPersonId = (
        SELECT
            TOP(1) PERSON_ID
        FROM
            PIMS_PERSON
        WHERE
            SURNAME = 'Rodriguez' AND FIRST_NAME = 'Manuel'
    );

SET
    @maheshPersonId = (
        SELECT
            TOP(1) PERSON_ID
        FROM
            PIMS_PERSON
        WHERE
            SURNAME = 'Babu' AND FIRST_NAME = 'Mahesh'
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
        'alesanch',
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
        'marodrig',
        '5D661D1E-14F0-477C-A7FB-31F21E8B4FDA',
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
        '672D88FC-61B9-4D6B-824A-83B5186E39C6',
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

DECLARE @systemAdmin bigint;
DECLARE @devinUserId bigint;
DECLARE @alejandroUserId bigint;
DECLARE @manuelUserId bigint;
DECLARE @maheshUserId bigint;

SET
    @systemAdmin = (
        SELECT
            TOP(1) ROLE_ID
        FROM
            PIMS_ROLE
        WHERE
            NAME = 'System Administrator'
    );

SET
    @devinUserId = (
        SELECT
            TOP(1) USER_ID
        FROM
            PIMS_USER
        WHERE
            BUSINESS_IDENTIFIER_VALUE = 'desmith'
    );
SET
    @alejandroUserId = (
        SELECT
            TOP(1) USER_ID
        FROM
            PIMS_USER
        WHERE
            BUSINESS_IDENTIFIER_VALUE = 'alesanch'
    );
SET
    @manuelUserId = (
        SELECT
            TOP(1) USER_ID
        FROM
            PIMS_USER
        WHERE
            BUSINESS_IDENTIFIER_VALUE = 'marodrig'
    );
SET
    @maheshUserId = (
        SELECT
            TOP(1) USER_ID
        FROM
            PIMS_USER
        WHERE
            BUSINESS_IDENTIFIER_VALUE = 'mbabu'
    );

INSERT INTO
    [dbo].[PIMS_USER_ROLE] (
        [USER_ID],
        [ROLE_ID],
        [APP_CREATE_USERID],
        [APP_CREATE_USER_DIRECTORY],
        [APP_LAST_UPDATE_USERID],
        [APP_LAST_UPDATE_USER_DIRECTORY],
        [DB_CREATE_USERID],
        [DB_LAST_UPDATE_USERID]
    )
VALUES
    (
        @devinUserId,
        @systemAdmin,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @alejandroUserId,
        @systemAdmin,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @manuelUserId,
        @systemAdmin,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @maheshUserId,
        @systemAdmin,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    );