PRINT N'Adding [PIMS_ROLE]'

INSERT INTO dbo.[PIMS_ROLE] (
    [ROLE_ID]
    , [ROLE_UID]
    , [NAME]
    , [DESCRIPTION]
    , [IS_PUBLIC]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
)
VALUES
(
    1
    , 'bbf27108-a0dc-4782-8025-7af7af711335'
    , 'System Administrator'
    , 'System Administrator of the PIMS solution.'
    , 0
    , 0
    , 0
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    2
    , '6ae8448d-5f0a-4607-803a-df0bc4efdc0f'
    , 'Agency Administrator'
    , 'Agency Administrator of the users agency.'
    , 0
    , 0
    , 0
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    3
    , 'aad8c03d-892c-4cc3-b992-5b41c4f2392c'
    , 'Real Estate Manager'
    , 'Real Estate Manager can manage properties within their agencies.'
    , 1
    , 0
    , 0
    , 'migration'
    , ''
    , 'migration'
    , ''
),
(
    4
    , '7a7b2549-ae85-4ad6-a8d3-3a5f8d4f9ca5'
    , 'Real Estate Analyst'
    , 'Real Estate Analyst can manage properties within their agencies.'
    , 1
    , 0
    , 0
    , 'migration'
    , ''
    , 'migration'
    , ''
)

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_ROLE_ID_SEQ]
RESTART WITH 5
