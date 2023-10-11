DECLARE @devinPersonId bigint;

DECLARE @alejandroPersonId bigint;

DECLARE @manuelPersonId bigint;

DECLARE @eduardoPersonId bigint;

DECLARE @amanPersonId bigint;

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
        'Herrera',
        'Eduardo',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        'Monga',
        'Aman',
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
    @eduardoPersonId = (
        SELECT
            TOP(1) PERSON_ID
        FROM
            PIMS_PERSON
        WHERE
            SURNAME = 'Herrera' AND FIRST_NAME = 'Eduardo'
    );

SET
    @amanPersonId = (
        SELECT
            TOP(1) PERSON_ID
        FROM
            PIMS_PERSON
        WHERE
            SURNAME = 'Monga' AND FIRST_NAME = 'Aman'
    );

INSERT INTO
    [dbo].[PIMS_USER] (
        [PERSON_ID],
        [BUSINESS_IDENTIFIER_VALUE],
        [GUID_IDENTIFIER_VALUE],
        [IS_DISABLED],
        [ISSUE_DATE],
        [USER_TYPE_CODE],
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
        'MINSTAFF',
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
        'MINSTAFF',
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
        'MINSTAFF',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @eduardoPersonId,
        'eherrera',
        '939A27D0-76CD-49B0-B474-53166ADB73DA',
        0,
        CURRENT_TIMESTAMP,
        'MINSTAFF',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @amanPersonId,
        'amonga',
        'E4240183-82D2-4CB7-BA7E-84E7067CB14A',
        0,
        CURRENT_TIMESTAMP,
        'MINSTAFF',
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
DECLARE @eduardoUserId bigint;
DECLARE @amanUserId bigint;

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
    @eduardoUserId = (
        SELECT
            TOP(1) USER_ID
        FROM
            PIMS_USER
        WHERE
            BUSINESS_IDENTIFIER_VALUE = 'eherrera'
    );
SET
    @amanUserId = (
        SELECT
            TOP(1) USER_ID
        FROM
            PIMS_USER
        WHERE
            BUSINESS_IDENTIFIER_VALUE = 'amonga'
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
        @eduardoUserId,
        @systemAdmin,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @amanUserId,
        @systemAdmin,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    );

DECLARE @southCoast bigint;

DECLARE @southInterior bigint;

DECLARE @northern bigint;

SET
    @southCoast = (
        SELECT
            TOP(1) REGION_CODE
        FROM
            PIMS_REGION
        WHERE
            REGION_NAME = 'South Coast Region'
    );

SET
    @southInterior = (
        SELECT
            TOP(1) REGION_CODE
        FROM
            PIMS_REGION
        WHERE
            REGION_NAME = 'Southern Interior Region'
    );

SET
    @northern = (
        SELECT
            TOP(1) REGION_CODE
        FROM
            PIMS_REGION
        WHERE
            REGION_NAME = 'Northern Region'
    );

INSERT INTO
    [dbo].[PIMS_REGION_USER] (
        [USER_ID],
        [REGION_CODE],
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
        @southCoast,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @devinUserId,
        @southInterior,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @devinUserId,
        @northern,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @alejandroUserId,
        @southCoast,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @alejandroUserId,
        @southInterior,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @alejandroUserId,
        @northern,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @manuelUserId,
        @southCoast,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @manuelUserId,
        @southInterior,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @manuelUserId,
        @northern,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @eduardoUserId,
        @southCoast,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @eduardoUserId,
        @southInterior,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @eduardoUserId,
        @northern,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @amanUserId,
        @southCoast,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @amanUserId,
        @southInterior,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    ),
    (
        @amanUserId,
        @northern,
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data',
        'seed_data'
    );
