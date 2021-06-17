PRINT N'Adding [PIMS_PROJECT_RISK]'

-- Parent Agencies.
INSERT INTO dbo.[PIMS_PROJECT_RISK] (
    [PROJECT_RISK_ID]
    , [CODE]
    , [NAME]
    , [DESCRIPTION]
    , [IS_DISABLED]
    , [SORT_ORDER]
) VALUES (
    1
    , 'GREEN'
    , 'Green'
    , '90-100% of the property value'
    , 0
    , 1
), (
    2
    , 'YELLOW'
    , 'Yellow'
    , '50% of the property value'
    , 0
    , 2
), (
    3
    , 'RED'
    , 'Red'
    , '0% of the property value'
    , 0
    , 3
)

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_PROJECT_RISK_ID_SEQ]
RESTART WITH 4
