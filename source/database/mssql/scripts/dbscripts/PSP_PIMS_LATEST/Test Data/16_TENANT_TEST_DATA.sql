-- Populate lease tenant data
INSERT INTO
    [dbo].[PIMS_LEASE_TENANT]
    (
    LEASE_ID,
    ORGANIZATION_ID,
    LESSOR_TYPE_CODE
    )
VALUES    
    (45, 2, 'ORG');

-- Insert contact methods
INSERT INTO
    PIMS_CONTACT_METHOD
    (
    CONTACT_METHOD_TYPE_CODE,
    PERSON_ID,
    CONTACT_METHOD_VALUE,
    APP_CREATE_USERID,
    APP_CREATE_USER_DIRECTORY,
    APP_LAST_UPDATE_USERID,
    APP_LAST_UPDATE_USER_DIRECTORY
    )
VALUES
    (
        'WORKMOBIL',
        1,
        '1111111111',
        'Test Data',
        'Test Data',
        'Test Data',
        'Test Data'
    );
INSERT INTO
    PIMS_CONTACT_METHOD
    (
    CONTACT_METHOD_TYPE_CODE,
    PERSON_ID,
    CONTACT_METHOD_VALUE,
    APP_CREATE_USERID,
    APP_CREATE_USER_DIRECTORY,
    APP_LAST_UPDATE_USERID,
    APP_LAST_UPDATE_USER_DIRECTORY
    )
VALUES
    (
        'WORKPHONE',
        2,
        '2222222222',
        'Test Data',
        'Test Data',
        'Test Data',
        'Test Data'
    );
INSERT INTO
    PIMS_CONTACT_METHOD
    (
    CONTACT_METHOD_TYPE_CODE,
    PERSON_ID,
    CONTACT_METHOD_VALUE,
    APP_CREATE_USERID,
    APP_CREATE_USER_DIRECTORY,
    APP_LAST_UPDATE_USERID,
    APP_LAST_UPDATE_USER_DIRECTORY
    )
VALUES
    (
        'PERSMOBIL',
        2,
        '3333333333',
        'Test Data',
        'Test Data',
        'Test Data',
        'Test Data'
    );

INSERT INTO
    PIMS_PERSON_ORGANIZATION
    (
    PERSON_ID,
    ORGANIZATION_ID,
    APP_CREATE_USERID,
    APP_CREATE_USER_DIRECTORY,
    APP_LAST_UPDATE_USERID,
    APP_LAST_UPDATE_USER_DIRECTORY
    )
VALUES
    (
        2,
        2,
        'Test Data',
        'Test Data',
        'Test Data',
        'Test Data'
    );

-- Update postal code
UPDATE
    PIMS_ADDRESS
SET
    POSTAL_CODE = 'V6Z 5G7',
    CONCURRENCY_CONTROL_NUMBER = [CONCURRENCY_CONTROL_NUMBER] + 1
WHERE
    ADDRESS_ID = 1;

-- Add notes for lease tenant
UPDATE
    [dbo].[PIMS_LEASE_TENANT]
SET
    [NOTE] = 'These are notes for the organization record',
    [CONCURRENCY_CONTROL_NUMBER] = [CONCURRENCY_CONTROL_NUMBER] + 1
WHERE
    [LEASE_TENANT_ID] = 1;
