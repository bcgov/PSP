PRINT N'Adding [PIMS_BUILDING_OCCUPANT_TYPE]'

INSERT INTO dbo.[PIMS_BUILDING_OCCUPANT_TYPE] (
    [BUILDING_OCCUPANT_TYPE_ID]
    , [NAME]
    , [IS_DISABLED]
) VALUES (
    1
    , 'Leased'
    , 0
), (
    2
    , 'Occupied By Owning Ministry'
    , 0
), (
    3
    , 'Unoccupied'
    , 0
)

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_BUILDING_OCCUPANT_TYPE_ID_SEQ]
RESTART WITH 4
