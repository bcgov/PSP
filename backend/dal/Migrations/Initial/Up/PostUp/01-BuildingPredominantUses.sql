PRINT N'Adding [PIMS_BUILDING_PREDOMINATE_USE]'

INSERT INTO dbo.[PIMS_BUILDING_PREDOMINATE_USE] (
    [BUILDING_PREDOMINATE_USE_ID]
    , [NAME]
    , [IS_DISABLED]
) VALUES (
    0
    , 'Religious'
    , 0
), (
    1
    , 'Research & Development Facility'
    , 0
), (
    2
    , 'Residential Detached'
    , 0
), (
    3
    , 'Residential Multi'
    , 0
), (
    4
    , 'Retail'
    , 0
), (
    5
    , 'Shelters / Orphanages / Children''s Homes / Halfway Homes'
    , 0
), (
    6
    , 'Social Assistance Housing'
    , 0
), (
    7
    , 'Storage'
    , 0
), (
    8
    , 'Storage Vehicle'
    , 0
), (
    9
    , 'Trailer Office'
    , 0
), (
    10
    , 'Trailer Other'
    , 0
), (
    11
    , 'University / College'
    , 0
), (
    12
    , 'Warehouse'
    , 0
), (
    13
    , 'Weigh Station'
    , 0
), (
    14
    , 'Marina'
    , 0
), (
    15
    , 'Jail / Prison'
    , 0
), (
    16
    , 'Community / Recreation Centre'
    , 0
), (
    17
    , 'Dormitory / Residence Halls'
    , 0
)

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_BUILDING_PREDOMINATE_USE_ID_SEQ]
RESTART WITH 18



