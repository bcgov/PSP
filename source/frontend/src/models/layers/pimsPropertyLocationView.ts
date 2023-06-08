import { FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';

export type stringDate = string;

// Source : Pims Geoserverview
export interface PIMS_Property_Location_View2 {
  readonly PROPERTY_ID: string | null;
  readonly PID: string | null;
  readonly PID_PADDED: string | null;
  readonly PIN: string | null;
  readonly PROPERTY_TYPE_CODE: string | null;
  readonly PROPERTY_STATUS_TYPE_CODE: string | null;
  readonly PROPERTY_DATA_SOURCE_TYPE_CODE: string | null;
  readonly PROPERTY_DATA_SOURCE_EFFECTIVE_DATE: string | null;
  readonly PROPERTY_CLASSIFICATION_TYPE_CODE: string | null;
  readonly PROPERTY_TENURE_TYPE_CODE: string | null;
  readonly STREET_ADDRESS_1: string | null;
  readonly STREET_ADDRESS_2: string | null;
  readonly STREET_ADDRESS_3: string | null;
  readonly MUNICIPALITY_NAME: string | null;
  readonly POSTAL_CODE: string | null;
  readonly PROVINCE_STATE_CODE: string | null;
  readonly PROVINCE_NAME: string | null;
  readonly COUNTRY_CODE: string | null;
  readonly COUNTRY_NAME: string | null;
  readonly NAME: string | null;
  readonly DESCRIPTION: string | null;
  readonly ADDRESS_ID: string | null;
  readonly REGION_CODE: string | null;
  readonly DISTRICT_CODE: string | null;
  readonly PROPERTY_AREA_UNIT_TYPE_CODE: string | null;
  readonly LAND_AREA: string | null;
  readonly LAND_LEGAL_DESCRIPTION: string | null;
  readonly ENCUMBRANCE_REASON: string | null;
  readonly IS_SENSITIVE: string | null;
  readonly IS_OWNED: string | null;
  readonly IS_PROPERTY_OF_INTEREST: string | null;
  readonly IS_VISIBLE_TO_OTHER_AGENCIES: string | null;
  readonly ZONING: string | null;
  readonly ZONING_POTENTIAL: string | null;
  readonly IS_PAYABLE_LEASE: string | null;
}

interface PIMS_Property_Location_View {
  readonly propertyID: string | null;
  readonly pid: string | null;
  readonly pidPadded: string | null;
  readonly pin: string | null;
  readonly propertyTypeCode: string | null;
  readonly propertyStatusTypeCode: string | null;
  readonly propertyDataSourceTypeCode: string | null;
  readonly propertyDataSourceEffectiveDate: string | null;
  readonly propertyClassificationTypeCode: string | null;
  readonly propertyTenureTypeCode: string | null;
  readonly streetAddress1: string | null;
  readonly streetAddress2: string | null;
  readonly streetAddress3: string | null;
  readonly municipalityName: string | null;
  readonly postalCode: string | null;
  readonly provinceStateCode: string | null;
  readonly provinceName: string | null;
  readonly countryCode: string | null;
  readonly countryName: string | null;
  readonly name: string | null;
  readonly description: string | null;
  readonly addressID: string | null;
  readonly regionCode: string | null;
  readonly districtCode: string | null;
  readonly propertyAreaUnitTypeCode: string | null;
  readonly landArea: string | null;
  readonly landLegalDescription: string | null;
  readonly encumbranceReason: string | null;
  readonly isSensitive: string | null;
  readonly isOwned: string | null;
  readonly isPropertyOfInterest: string | null;
  readonly isVisibleToOtherAgencies: string | null;
  readonly zoning: string | null;
  readonly zoningPotential: string | null;
  readonly isPayableLease: string | null;
}

const propertyLocationFromFeatureCollection = (
  values: FeatureCollection<Geometry, PIMS_Property_Location_View2> | undefined,
): PIMS_Property_Location_View[] | undefined =>
  values?.features
    ?.filter(feature => feature?.geometry?.type === 'Point')
    .map((feature): PIMS_Property_Location_View | undefined => {
      return fromProperties(feature.properties);
    })
    .filter((x): x is PIMS_Property_Location_View => !!x);

function fromProperties(
  properties: PIMS_Property_Location_View2,
): PIMS_Property_Location_View | undefined {
  if (properties === null || properties === undefined) {
    return undefined;
  }

  return {
    propertyID: properties.PROPERTY_ID,
    pid: properties.PID,
    pidPadded: properties.PID_PADDED,
    pin: properties.PIN,
    propertyTypeCode: properties.PROPERTY_TYPE_CODE,
    propertyStatusTypeCode: properties.PROPERTY_STATUS_TYPE_CODE,
    propertyDataSourceTypeCode: properties.PROPERTY_DATA_SOURCE_TYPE_CODE,
    propertyDataSourceEffectiveDate: properties.PROPERTY_DATA_SOURCE_EFFECTIVE_DATE,
    propertyClassificationTypeCode: properties.PROPERTY_CLASSIFICATION_TYPE_CODE,
    propertyTenureTypeCode: properties.PROPERTY_TENURE_TYPE_CODE,
    streetAddress1: properties.STREET_ADDRESS_1,
    streetAddress2: properties.STREET_ADDRESS_2,
    streetAddress3: properties.STREET_ADDRESS_3,
    municipalityName: properties.MUNICIPALITY_NAME,
    postalCode: properties.POSTAL_CODE,
    provinceStateCode: properties.PROVINCE_STATE_CODE,
    provinceName: properties.PROVINCE_NAME,
    countryCode: properties.COUNTRY_CODE,
    countryName: properties.COUNTRY_NAME,
    name: properties.NAME,
    description: properties.DESCRIPTION,
    addressID: properties.ADDRESS_ID,
    regionCode: properties.REGION_CODE,
    districtCode: properties.DISTRICT_CODE,
    propertyAreaUnitTypeCode: properties.PROPERTY_AREA_UNIT_TYPE_CODE,
    landArea: properties.LAND_AREA,
    landLegalDescription: properties.LAND_LEGAL_DESCRIPTION,
    encumbranceReason: properties.ENCUMBRANCE_REASON,
    isSensitive: properties.IS_SENSITIVE,
    isOwned: properties.IS_OWNED,
    isPropertyOfInterest: properties.IS_PROPERTY_OF_INTEREST,
    isVisibleToOtherAgencies: properties.IS_VISIBLE_TO_OTHER_AGENCIES,
    zoning: properties.ZONING,
    zoningPotential: properties.ZONING_POTENTIAL,
    isPayableLease: properties.IS_PAYABLE_LEASE,
  };
}
