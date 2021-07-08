PRINT N'Adding [PIMS_USER]'

INSERT INTO dbo.[PIMS_USER] (
    [USER_UID]
    , [USERNAME]
    , [DISPLAY_NAME]
    , [FIRST_NAME]
    , [LAST_NAME]
    , [EMAIL]
    , [IS_DISABLED]
    , [IS_SYSTEM]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
) VALUES (
    '00000000-0000-0000-0000-000000000000'
    , 'system'
    , 'system'
    , 'system'
    , 'system'
    , 'pims@Pims.gov.bc.ca'
    , 1
    , 1
    , 'migration'
    , ''
    , 'migration'
    , ''
)

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_USER_ID_SEQ]
RESTART WITH 2
