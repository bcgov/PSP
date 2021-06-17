SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- This script provides a way to union both parcels and buildings into a single result of properties.
-- Updating view to support many-to-many parcels and buildings.TEMPORARY
CREATE VIEW [dbo].[PROPERTY_VW] AS
SELECT
    p.[PARCEL_ID] AS [ID]
    , p.[CONCURRENCY_CONTROL_NUMBER] AS [RowVersion]
    , p.[PROPERTY_TYPE_ID] AS [PropertyTypeId]
    , p.[PROPERTY_CLASSIFICATION_ID] AS [ClassificationId]
    , [Classification] = c.[NAME]
    , p.[AGENCY_ID] AS [AgencyId]
    , [Agency] = ISNULL(pa.[NAME], a.[NAME])
    , [AgencyCode] = ISNULL(pa.[CODE], a.[CODE])
    , [SubAgency] = (
        SELECT CASE WHEN pa.[AGENCY_ID] IS NOT NULL
            THEN a.[NAME]
            ELSE NULL
        END)
    , [SubAgencyCode] = (
        SELECT CASE WHEN pa.[AGENCY_ID] IS NOT NULL
            THEN a.[CODE]
            ELSE NULL
        END)
    , p.[ADDRESS_ID] AS [AddressId]
    , [Address] = TRIM(ISNULL(adr.[ADDRESS1], '') + ' ' + ISNULL(adr.[ADDRESS2], ''))
    , [AdministrativeArea] = adr.[ADMINISTRATIVE_AREA]
    , [Province] = ap.[NAME]
    , adr.[POSTAL] AS [Postal]
    , p.[PROJECT_NUMBERS] AS [ProjectNumbers]
    , p.[NAME] AS [Name]
    , p.[DESCRIPTION] AS [Description]
    , p.[LOCATION] AS [Location]
    , p.[BOUNDARY] AS [Boundary]
    , p.[IS_SENSITIVE] AS [IsSensitive]
    , p.[IS_VISIBLE_TO_OTHER_AGENCIES] AS [IsVisibleToOtherAgencies]

    -- Parcel Properties
    , p.[PID]
    , p.[PIN]
    , [LandArea] = p.[LAND_AREA]
    , [LandLegalDescription] = p.[LAND_LEGAL_DESCRIPTION]
    , [Zoning] = p.[ZONING]
    , [ZoningPotential] = p.[ZONING_POTENTIAL]

    -- Building Properties
    , [ParcelId] = p.[PARCEL_ID]
    , [BuildingConstructionTypeId] = 0
    , [BuildingConstructionType] = null
    , [BuildingFloorCount] = 0
    , [BuildingPredominateUseId] = 0
    , [BuildingPredominateUse] = null
    , [BuildingOccupantTypeId] = 0
    , [BuildingOccupantType] = null
    , [BuildingTenancy] = null
    , [RentableArea] = 0
    , [LeaseExpiry] = null
    , [OccupantName] = null
    , [TransferLeaseOnSale] = CAST(0 AS BIT)

    , [AssessedLand] = eas.[VALUE]
    , [AssessedLandDate] = eas.[DATE]
    , [AssessedBuilding] = eim.[VALUE]
    , [AssessedBuildingDate] = eim.[DATE]
    , [Market] = ISNULL(fe.[VALUE], 0)
    , [MarketFiscalYear] = fe.[FISCAL_YEAR]
    , [NetBook] = ISNULL(fn.[VALUE], 0)
    , [NetBookFiscalYear] = fn.[FISCAL_YEAR]
FROM dbo.[PIMS_PARCEL] p
LEFT JOIN (SELECT DISTINCT [SUBDIVISION_PARCEL_ID] FROM dbo.[PIMS_PARCEL_PARCEL]) sp ON p.[PARCEL_ID] = sp.[SUBDIVISION_PARCEL_ID]
JOIN dbo.[PIMS_PROPERTY_CLASSIFICATION] c ON p.[PROPERTY_CLASSIFICATION_ID] = c.[PROPERTY_CLASSIFICATION_ID]
LEFT JOIN dbo.[PIMS_AGENCY] a ON p.[AGENCY_ID] = a.[AGENCY_ID]
LEFT JOIN dbo.[PIMS_AGENCY] pa ON a.[PARENT_AGENCY_ID] = pa.[AGENCY_ID]
JOIN dbo.[PIMS_ADDRESS] adr ON p.[ADDRESS_ID] = adr.[ADDRESS_ID]
JOIN dbo.[PIMS_PROVINCE] ap ON adr.[PROVINCE_ID] = ap.[PROVINCE_ID]
OUTER APPLY (
    SELECT TOP 1
        [VALUE]
        , [DATE]
    FROM dbo.[PIMS_PARCEL_EVALUATION]
    WHERE [PARCEL_ID] = p.[PARCEL_ID]
        AND [KEY] = 0 -- Assessed Land
    ORDER BY [DATE] DESC
) AS eas
OUTER APPLY (
    SELECT TOP 1
        [VALUE]
        , [DATE]
    FROM dbo.[PIMS_PARCEL_EVALUATION]
    WHERE [PARCEL_ID] = p.[PARCEL_ID]
        AND [KEY] = 2 -- Assessed Building
    ORDER BY [DATE] DESC
) AS eim
OUTER APPLY (
    SELECT TOP 1
        [VALUE]
        , [FISCAL_YEAR]
    FROM dbo.[PIMS_PARCEL_FISCAL]
    WHERE [PARCEL_ID] = p.[PARCEL_ID]
        AND [KEY] = 1 -- Market
    ORDER BY [FISCAL_YEAR] DESC
) AS fe
OUTER APPLY (
    SELECT TOP 1
        [VALUE]
        , [FISCAL_YEAR]
    FROM dbo.[PIMS_PARCEL_FISCAL]
    WHERE [PARCEL_ID] = p.[PARCEL_ID]
        AND [KEY] = 0 -- NetBook
    ORDER BY [FISCAL_YEAR] DESC
) AS fn
UNION ALL
SELECT
    b.[BUILDING_ID]
    , b.[CONCURRENCY_CONTROL_NUMBER] AS [RowVersion]
    , b.[PROPERTY_TYPE_ID] AS [PropertyTypeId]
    , b.[PROPERTY_CLASSIFICATION_ID] AS [ClassificationId]
    , [Classification] = c.[NAME]
    , b.[AGENCY_ID] AS [AgencyId]
    , [Agency] = ISNULL(pa.[NAME], a.[NAME])
    , [AgencyCode] = ISNULL(pa.[CODE], a.[CODE])
    , [SubAgency] = (
        SELECT CASE WHEN pa.[AGENCY_ID] IS NOT NULL
            THEN a.[NAME]
            ELSE NULL
        END)
    , [SubAgencyCode] = (
        SELECT CASE WHEN pa.[AGENCY_ID] IS NOT NULL
            THEN a.[CODE]
            ELSE NULL
        END)
    , b.[ADDRESS_ID] AS [AddressId]
    , [Address] = TRIM(ISNULL(adr.[ADDRESS1], '') + ' ' + ISNULL(adr.[ADDRESS2], ''))
    , [AdministrativeArea] = adr.[ADMINISTRATIVE_AREA]
    , [Province] = ap.[NAME]
    , adr.[POSTAL] AS [Postal]
    , b.[PROJECT_NUMBERS] AS [ProjectNumbers]
    , b.[NAME] AS [Name]
    , b.[DESCRIPTION] AS [Description]
    , b.[LOCATION] AS [Location]
    , b.[BOUNDARY] AS [Boundary]
    , b.[IS_SENSITIVE] AS [IsSensitive]
    , b.[IS_VISIBLE_TO_OTHER_AGENCIES] AS [IsVisibleToOtherAgencies]

    -- Parcel Properties
    , [PID] = p.[PID]
    , [PIN] = p.[PIN]
    , [LandArea] = p.[LAND_AREA]
    , [LandLegalDescription] = p.[LAND_LEGAL_DESCRIPTION]
    , [Zoning] = p.[ZONING]
    , [ZoningPotential] = p.[ZONING_POTENTIAL]

    -- Building Properties
    , [ParcelId] = p.[PARCEL_ID]
    , [BuildingConstructionTypeId] = b.[BUILDING_CONSTRUCTION_TYPE_ID]
    , [BuildingConstructionType] = bct.[NAME]
    , [BuildingFloorCount] = b.[BUILDING_FLOOR_COUNT]
    , [BuildingPredominateUseId] = b.[BUILDING_PREDOMINATE_USE_ID]
    , [BuildingPredominateUse] = bpu.[NAME]
    , [BuildingOccupantTypeId] = b.[BUILDING_OCCUPANT_TYPE_ID]
    , [BuildingOccupantType] = bot.[NAME]
    , [BuildingTenancy] = b.[BUILDING_TENANCY]
    , [RentableArea] = b.[RENTABLE_AREA]
    , [LeaseExpiry] = b.[LEASE_EXPIRY]
    , [OccupantName] = b.[OCCUPANT_NAME]
    , [TransferLeaseOnSale] = b.[TRANSFER_LEASE_ON_SALE]

    , [AssessedLand] = null
    , [AssessedLandDate] = null
    , [AssessedBuilding] = eim.[VALUE]
    , [AssessedBuildingDate] = eim.[DATE]
    , [Market] = ISNULL(fe.[VALUE], 0)
    , [MarketFiscalYear] = fe.[FISCAL_YEAR]
    , [NetBook] = ISNULL(fn.[VALUE], 0)
    , [NetBookFiscalYear] = fn.[FISCAL_YEAR]
FROM dbo.[PIMS_BUILDING] b
LEFT JOIN dbo.[PIMS_PARCEL_BUILDING] pb ON b.[BUILDING_ID] = pb.[BUILDING_ID]
LEFT JOIN dbo.[PIMS_PARCEL] p ON pb.[PARCEL_ID] = p.[PARCEL_ID]
JOIN dbo.[PIMS_PROPERTY_CLASSIFICATION] c ON b.[PROPERTY_CLASSIFICATION_ID] = c.[PROPERTY_CLASSIFICATION_ID]
LEFT JOIN dbo.[PIMS_AGENCY] a ON b.[AGENCY_ID] = a.[AGENCY_ID]
LEFT JOIN dbo.[PIMS_AGENCY] pa ON a.[PARENT_AGENCY_ID] = pa.[AGENCY_ID]
JOIN dbo.[PIMS_ADDRESS] adr ON b.[ADDRESS_ID] = adr.[ADDRESS_ID]
JOIN dbo.[PIMS_PROVINCE] ap ON adr.[PROVINCE_ID] = ap.[PROVINCE_ID]
JOIN dbo.[PIMS_BUILDING_CONSTRUCTION_TYPE] bct ON b.[BUILDING_CONSTRUCTION_TYPE_ID] = bct.[BUILDING_CONSTRUCTION_TYPE_ID]
JOIN dbo.[PIMS_BUILDING_OCCUPANT_TYPE] bot ON b.[BUILDING_OCCUPANT_TYPE_ID] = bot.[BUILDING_OCCUPANT_TYPE_ID]
JOIN dbo.[PIMS_BUILDING_PREDOMINATE_USE] bpu ON b.[BUILDING_PREDOMINATE_USE_ID] = bpu.[BUILDING_PREDOMINATE_USE_ID]
OUTER APPLY (
    SELECT TOP 1
        [VALUE]
        , [DATE]
    FROM dbo.[PIMS_BUILDING_EVALUATION]
    WHERE [BUILDING_ID] = b.[BUILDING_ID]
        AND [KEY] = 0 -- Assessed
    ORDER BY [DATE] DESC
) AS eim
OUTER APPLY (
    SELECT TOP 1
        [VALUE]
        , [FISCAL_YEAR]
    FROM dbo.[PIMS_BUILDING_FISCAL]
    WHERE [BUILDING_ID] = b.[BUILDING_ID]
        AND [KEY] = 1 -- Market
    ORDER BY [FISCAL_YEAR] DESC
) AS fe
OUTER APPLY (
    SELECT TOP 1
        [VALUE]
        , [FISCAL_YEAR]
    FROM dbo.[PIMS_BUILDING_FISCAL]
    WHERE [BUILDING_ID] = b.[BUILDING_ID]
        AND [KEY] = 0 -- NetBook
    ORDER BY [FISCAL_YEAR] DESC
) AS fn
GO
