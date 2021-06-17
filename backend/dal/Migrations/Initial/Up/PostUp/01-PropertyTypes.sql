PRINT N'Adding [PIMS_PROPERTY_TYPE]'

INSERT INTO dbo.[PIMS_PROPERTY_TYPE] (
    [PROPERTY_TYPE_ID]
    , [NAME]
    , [IS_DISABLED]
) VALUES (
    0
    , 'Land'
    , 0
), (
    1
    , 'Building'
    , 0
)

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_PROPERTY_TYPE_ID_SEQ]
RESTART WITH 2
