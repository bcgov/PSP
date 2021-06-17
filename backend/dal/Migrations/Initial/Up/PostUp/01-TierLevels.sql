PRINT N'Adding [PIMS_TIER_LEVEL]'

INSERT INTO dbo.[PIMS_TIER_LEVEL] (
    [TIER_LEVEL_ID]
    , [NAME]
    , [IS_DISABLED]
    , [DESCRIPTION]
    , [SORT_ORDER]
) VALUES (
    1
    , 'Tier 1'
    , 0
    , 'Properties with a net value of less than $1M.'
    , 1
), (
    2
    , 'Tier 2'
    , 0
    , 'Properties with a net value of $1M or more and less than $10M'
    , 2
), (
    3
    , 'Tier 3'
    , 0
    , 'Properties from a single parcels with a net value of $10M or more'
    , 3
)
, (
    4
    , 'Tier 4'
    , 0
    , 'Properties from multiple parcels with a cumulative net value of $10M or more'
    , 4
)

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_TIER_LEVEL_ID_SEQ]
RESTART WITH 5
