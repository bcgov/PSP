PRINT 'Adding ProjectRisks'

SET IDENTITY_INSERT dbo.[ProjectRisks] ON

-- Parent Agencies.
INSERT INTO dbo.[ProjectRisks] (
    [Id]
    , [Code]
    , [Name]
    , [Description]
    , [IsDisabled]
    , [SortOrder]
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

SET IDENTITY_INSERT dbo.[ProjectRisks] OFF
