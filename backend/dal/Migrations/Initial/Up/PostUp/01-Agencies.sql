PRINT N'Adding [PIMS_AGENCY]'

MERGE INTO dbo.[PIMS_AGENCY] dest
USING (
    VALUES (
        1
        , null
        , 'TRAN'
        , 'Ministry of Transportation and Infrastructure'
        , 0
        , 1
        , 'nancy.bain@gov.bc.ca'
        , 'ADM & EFO Finance & Management Services Department'
    ), (
        2
        , 1
        , 'BCTFA'
        , 'BC Transportation Financing Authority'
        , 0
        , 0
        , null
        , null
    ), (
        3
        , 1
        , 'BCT'
        , 'BC Transit'
        , 0
        , 0
        , null
        , null
    )
) AS src (
    [AGENCY_ID]
    , [PARENT_AGENCY_ID]
    , [CODE]
    , [NAME]
    , [IS_DISABLED]
    , [SEND_EMAIL]
    , [EMAIL]
    , [ADDRESS_TO]
)
ON dest.[AGENCY_ID] = src.[AGENCY_ID]
WHEN MATCHED THEN
    UPDATE SET
        dest.[PARENT_AGENCY_ID] = src.[PARENT_AGENCY_ID]
        , dest.[CODE] = src.[CODE]
        , dest.[NAME] = src.[NAME]
        , dest.[IS_DISABLED] = src.[IS_DISABLED]
        , dest.[SEND_EMAIL] = src.[SEND_EMAIL]
        , dest.[EMAIL] = src.[EMAIL]
        , dest.[ADDRESS_TO] = src.[ADDRESS_TO]
        , dest.[APP_CREATE_USERID] = 'migration'
        , dest.[APP_CREATE_USER_DIRECTORY] = ''
        , dest.[APP_LAST_UPDATE_USERID] = 'migration'
        , dest.[APP_LAST_UPDATE_USER_DIRECTORY] = ''
WHEN NOT MATCHED THEN
    INSERT (
        [AGENCY_ID]
        , [PARENT_AGENCY_ID]
        , [CODE]
        , [NAME]
        , [IS_DISABLED]
        , [SEND_EMAIL]
        , [EMAIL]
        , [ADDRESS_TO]
        , [APP_CREATE_USERID]
        , [APP_CREATE_USER_DIRECTORY]
        , [APP_LAST_UPDATE_USERID]
        , [APP_LAST_UPDATE_USER_DIRECTORY]
    ) VALUES (
        src.[AGENCY_ID]
        , src.[PARENT_AGENCY_ID]
        , src.[CODE]
        , src.[NAME]
        , src.[IS_DISABLED]
        , src.[SEND_EMAIL]
        , src.[EMAIL]
        , src.[ADDRESS_TO]
        , 'migration'
        , ''
        , 'migration'
        , ''
    );

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_AGENCY_ID_SEQ]
RESTART WITH 4
