import { Geometry } from 'geojson';

export type stringDate = string;

// Source : Pims Geoserverview
// name: PIMS_PROPERTY_LOCATION_VW
export interface PIMS_Property_Location_View {
  readonly PROPERTY_ID: number | null;
  readonly PID: number | null;
  readonly PID_PADDED: string | null;
  readonly PIN: number | null;
  readonly PROPERTY_TYPE_CODE: string | null;
  readonly PROPERTY_STATUS_TYPE_CODE: string | null;
  readonly PROPERTY_DATA_SOURCE_TYPE_CODE: string | null;
  readonly PROPERTY_DATA_SOURCE_EFFECTIVE_DATE: stringDate | null;
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
  readonly ADDRESS_ID: number | null;
  readonly REGION_CODE: number | null;
  readonly DISTRICT_CODE: number | null;
  //readonly GEOMETRY: Geometry | null;
  readonly PROPERTY_AREA_UNIT_TYPE_CODE: string | null;
  readonly LAND_AREA: number | null;
  readonly LAND_LEGAL_DESCRIPTION: string | null;
  readonly SURVEY_PLAN_NUMBER: string | null;
  readonly IS_OWNED: boolean | null;
  readonly IS_RETIRED: boolean | null;
  readonly IS_DISPOSED: boolean | null;
  readonly IS_OTHER_INTEREST: boolean | null;
  readonly HAS_ACTIVE_ACQUISITION_FILE: boolean | null;
  readonly HAS_ACTIVE_RESEARCH_FILE: boolean | null;
  readonly IS_PAYABLE_LEASE: boolean | null;
  readonly IS_ACTIVE_PAYABLE_LEASE: boolean | null;
  readonly IS_RECEIVABLE_LEASE: boolean | null;
  readonly IS_ACTIVE_RECEIVABLE_LEASE: boolean | null;
  readonly HISTORICAL_FILE_NUMBER_STR: string | null;
}

export const emptyPropertyLocation: PIMS_Property_Location_View = {
  PROPERTY_ID: null,
  PID: null,
  PID_PADDED: null,
  PIN: null,
  PROPERTY_TYPE_CODE: null,
  PROPERTY_STATUS_TYPE_CODE: null,
  PROPERTY_DATA_SOURCE_TYPE_CODE: null,
  PROPERTY_DATA_SOURCE_EFFECTIVE_DATE: null,
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
  ADDRESS_ID: null,
  REGION_CODE: null,
  DISTRICT_CODE: null,
  //GEOMETRY: null,
  PROPERTY_AREA_UNIT_TYPE_CODE: null,
  LAND_AREA: null,
  LAND_LEGAL_DESCRIPTION: null,
  SURVEY_PLAN_NUMBER: null,
  IS_OWNED: null,
  IS_DISPOSED: null,
  HAS_ACTIVE_ACQUISITION_FILE: null,
  HAS_ACTIVE_RESEARCH_FILE: null,
  IS_OTHER_INTEREST: null,
  IS_RETIRED: null,
  IS_PAYABLE_LEASE: null,
  IS_ACTIVE_PAYABLE_LEASE: null,
  IS_RECEIVABLE_LEASE: null,
  IS_ACTIVE_RECEIVABLE_LEASE: null,
  HISTORICAL_FILE_NUMBER_STR: null,
};

// Source : Pims Geoserverview
// name: PIMS_PROPERTY_LOCATION_LITE_VW
export interface PIMS_Property_Location_Lite_View {
  readonly PROPERTY_ID: number | null;
  //GEOMETRY/1
  readonly IS_OWNED: boolean | null;
  readonly IS_RETIRED: boolean | null;
  readonly IS_DISPOSED: boolean | null;
  readonly IS_OTHER_INTEREST: boolean | null;
  readonly HAS_ACTIVE_ACQUISITION_FILE: boolean | null;
  readonly HAS_ACTIVE_RESEARCH_FILE: boolean | null;
}

// Source : Pims Geoserverview
// name: PIMS_PROPERTY_BOUNDARY_VW
export interface PIMS_Property_Boundary_View {
  readonly PROPERTY_ID: number | null;
  readonly PID: number | null;
  readonly PID_PADDED: string | null;
  readonly PIN: number | null;
  readonly PROPERTY_TYPE_CODE: string | null;
  readonly PROPERTY_STATUS_TYPE_CODE: string | null;
  readonly PROPERTY_DATA_SOURCE_TYPE_CODE: string | null;
  readonly PROPERTY_DATA_SOURCE_EFFECTIVE_DATE: stringDate | null;
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
  readonly ADDRESS_ID: number | null;
  readonly REGION_CODE: number | null;
  readonly DISTRICT_CODE: number | null;
  readonly GEOMETRY: Geometry | null;
  readonly PROPERTY_AREA_UNIT_TYPE_CODE: string | null;
  readonly LAND_AREA: number | null;
  readonly LAND_LEGAL_DESCRIPTION: string | null;
  readonly SURVEY_PLAN_NUMBER: string | null;
  readonly IS_OWNED: boolean | null;
  readonly IS_RETIRED: boolean | null;
  readonly IS_DISPOSED: boolean | null;
  readonly IS_OTHER_INTEREST: boolean | null;
  readonly HAS_ACTIVE_ACQUISITION_FILE: boolean | null;
  readonly HAS_ACTIVE_RESEARCH_FILE: boolean | null;
  readonly IS_PAYABLE_LEASE: boolean | null;
  readonly IS_ACTIVE_PAYABLE_LEASE: boolean | null;
  readonly IS_RECEIVABLE_LEASE: boolean | null;
  readonly IS_ACTIVE_RECEIVABLE_LEASE: boolean | null;
  readonly HISTORICAL_FILE_NUMBER_STR: string | null;
}

// Source : Pims Geoserverview
// name: PIMS_PROPERTY_LITE_VW
export interface PIMS_Property_Lite_View {
  readonly LOCATION: Geometry | null;
  readonly BOUNDARY: Geometry | null;
  readonly IS_OWNED: boolean | null;
  readonly IS_RETIRED: boolean | null;
  readonly IS_DISPOSED: boolean | null;
  readonly IS_OTHER_INTEREST: boolean | null;
  readonly HAS_ACTIVE_ACQUISITION_FILE: boolean | null;
  readonly HAS_ACTIVE_RESEARCH_FILE: boolean | null;
}
