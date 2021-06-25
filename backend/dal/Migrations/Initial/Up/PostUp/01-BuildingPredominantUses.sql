PRINT N'Adding [PIMS_BUILDING_PREDOMINATE_USE]'

INSERT INTO dbo.[PIMS_BUILDING_PREDOMINATE_USE] (
    [BUILDING_PREDOMINATE_USE_ID]
    , [NAME]
    , [IS_DISABLED]
) VALUES (
    1
    , 'Religious'
    , 0
), (
    2
    , 'Research & Development Facility'
    , 0
), (
    3
    , 'Residential Detached'
    , 0
), (
    4
    , 'Residential Multi'
    , 0
), (
    5
    , 'Retail'
    , 0
), (
    6
    , 'Shelters / Orphanages / Children''s Homes / Halfway Homes'
    , 0
), (
    7
    , 'Social Assistance Housing'
    , 0
), (
    8
    , 'Storage'
    , 0
), (
    9
    , 'Storage Vehicle'
    , 0
), (
    10
    , 'Trailer Office'
    , 0
), (
    11
    , 'Trailer Other'
    , 0
), (
    12
    , 'University / College'
    , 0
), (
    13
    , 'Warehouse'
    , 0
), (
    14
    , 'Weigh Station'
    , 0
), (
    15
    , 'Marina'
    , 0
), (
    16
    , 'Jail / Prison'
    , 0
), (
    17
    , 'Community / Recreation Centre'
    , 0
), (
    18
    , 'Dormitory / Residence Halls'
    , 0
)

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_BUILDING_PREDOMINATE_USE_ID_SEQ]
RESTART WITH 19



