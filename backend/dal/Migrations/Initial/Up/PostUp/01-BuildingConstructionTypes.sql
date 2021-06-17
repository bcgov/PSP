PRINT N'Adding [PIMS_BUILDING_CONSTRUCTION_TYPE]'

INSERT INTO dbo.[PIMS_BUILDING_CONSTRUCTION_TYPE] (
    [BUILDING_CONSTRUCTION_TYPE_ID]
    , [NAME]
    , [IS_DISABLED]
) VALUES (
    0
    , 'Concrete'
    , 0
), (
    1
    , 'Masonry'
    , 0
), (
    2
    , 'Mixed'
    , 0
), (
    3
    , 'Steel'
    , 0
), (
    4
    , 'Wood'
    , 0
)

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_BUILDING_CONSTRUCTION_TYPE_ID_SEQ]
RESTART WITH 5
