PRINT N'Adding [PIMS_PROPERTY_CLASSIFICATION]'

INSERT INTO dbo.[PIMS_PROPERTY_CLASSIFICATION] (
    [PROPERTY_CLASSIFICATION_ID]
    , [NAME]
    , [IS_DISABLED]
    , [IS_VISIBLE]
    , [DISPLAY_ORDER]
) VALUES (
    1
    , 'Core Operational'
    , 0
    , 1
    , 1
), (
    2
    , 'Core Strategic'
    , 0
    , 1
    , 2
), (
    3
    , 'Surplus Active'
    , 0
    , 1
    , 3
), (
    4
    , 'Surplus Encumbered'
    , 0
    , 1
    , 4
), (
    5
    , 'Disposed'
    , 0
    , 0
    , 5
)

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_PROPERTY_CLASSIFICATION_ID_SEQ]
RESTART WITH 6
