export type stringDate = string;

// Source : Pims Geoserverview
// name: PIMS_PROPERTY_LOCATION_VW
export interface PIMS_Property_Location_View {
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
  readonly SURVEY_PLAN_NUMBER: string | null;
  readonly ENCUMBRANCE_REASON: string | null;
  readonly IS_SENSITIVE: string | null;
  readonly IS_OWNED: string | null;
  readonly IS_PROPERTY_OF_INTEREST: string | null;
  readonly IS_VISIBLE_TO_OTHER_AGENCIES: string | null;
  readonly ZONING: string | null;
  readonly ZONING_POTENTIAL: string | null;
  readonly IS_PAYABLE_LEASE: string | null;
}

export const EmptyPropertyLocation: PIMS_Property_Location_View = {
  PROPERTY_ID: null,
  PID: null,
  PID_PADDED: null,
  PIN: null,
  PROPERTY_TYPE_CODE: null,
  PROPERTY_STATUS_TYPE_CODE: null,
  PROPERTY_DATA_SOURCE_TYPE_CODE: null,
  PROPERTY_DATA_SOURCE_EFFECTIVE_DATE: null,
  PROPERTY_CLASSIFICATION_TYPE_CODE: null,
  PROPERTY_TENURE_TYPE_CODE: null,
  STREET_ADDRESS_1: null,
  STREET_ADDRESS_2: null,
  STREET_ADDRESS_3: null,
  MUNICIPALITY_NAME: null,
  POSTAL_CODE: null,
  PROVINCE_STATE_CODE: null,
  PROVINCE_NAME: null,
  COUNTRY_CODE: null,
  COUNTRY_NAME: null,
  NAME: null,
  DESCRIPTION: null,
  ADDRESS_ID: null,
  REGION_CODE: null,
  DISTRICT_CODE: null,
  PROPERTY_AREA_UNIT_TYPE_CODE: null,
  LAND_AREA: null,
  LAND_LEGAL_DESCRIPTION: null,
  SURVEY_PLAN_NUMBER: null,
  ENCUMBRANCE_REASON: null,
  IS_SENSITIVE: null,
  IS_OWNED: null,
  IS_PROPERTY_OF_INTEREST: null,
  IS_VISIBLE_TO_OTHER_AGENCIES: null,
  ZONING: null,
  ZONING_POTENTIAL: null,
  IS_PAYABLE_LEASE: null,
};

// Source : Pims Geoserverview
// name: PIMS_PROPERTY_BOUNDARY_VW
export interface PIMS_Property_Boundary_View {
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
  readonly GEOMETRY: string | null;
  readonly PROPERTY_AREA_UNIT_TYPE_CODE: string | null;
  readonly LAND_AREA: string | null;
  readonly LAND_LEGAL_DESCRIPTION: string | null;
  readonly SURVEY_PLAN_NUMBER: string | null;
  readonly ENCUMBRANCE_REASON: string | null;
  readonly IS_SENSITIVE: string | null;
  readonly IS_OWNED: string | null;
  readonly IS_PROPERTY_OF_INTEREST: string | null;
  readonly IS_VISIBLE_TO_OTHER_AGENCIES: string | null;
  readonly ZONING: string | null;
  readonly ZONING_POTENTIAL: string | null;
  readonly IS_PAYABLE_LEASE: string | null;
}
