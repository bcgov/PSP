PRINT N'Adding [PIMS_PROVINCE]'

INSERT INTO dbo.[PIMS_PROVINCE] (
    [PROVINCE_ID]
    , [CODE]
    , [NAME]
) VALUES (
    1
    , 'BC'
    , 'British Columbia'
), (
    2
    , 'ON'
    , 'Ontario'
)

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_PROVINCE_ID_SEQ]
RESTART WITH 3
