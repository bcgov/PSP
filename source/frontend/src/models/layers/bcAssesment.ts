export enum BC_ASSESSMENT_TYPES {
  LEGAL_DESCRIPTION = 'LEGAL_DESCRIPTION',
  ADDRESSES = 'ADDRESSES',
  VALUES = 'VALUES',
  CHARGES = 'CHARGES',
  FOLIO_DESCRIPTION = 'FOLIO_DESCRIPTION',
  SALES = 'SALES',
  CHARACTERISTICS = 'CHARACTERISTICS',
}

export interface IBcAssessmentSummary {
  FOLIO_DESCRIPTION: Partial<{
    BCA_FD_SYSID: number;
    ROLL_NUMBER: string;
    FOLIO_ID: string;
    FOLIO_STATUS: string;
    FOLIO_STATUS_DESCRIPTION: string;
    ACTUAL_USE_CODE: string;
    ACTUAL_USE_DESCRIPTION: string;
    ALR_CODE: string;
    ALR_DESCRIPTION: string;
    BC_TRANSIT_IND: string;
    LAND_DEPTH: number;
    LAND_SIZE: number;
    LAND_DIMENSION_TYPE: string;
    LAND_UNITS: string;
    LAND_WIDTH: number;
    NEIGHBOURHOOD_CODE: string;
    NEIGHBOURHOOD: string;
    MANUAL_CLASS_CODE: string;
    MANUAL_CLASS_DESCRIPTION: string;
    REGIONAL_DISTRICT_CODE: string;
    REGIONAL_DISTRICT: string;
    HOSPITAL_DISTRICT_CODE: string;
    HOSPITAL_DISTRICT: string;
    SCHOOL_DISTRICT_CODE: string;
    SCHOOL_DISTRICT: string;
    TENURE_CODE: string;
    TENURE_DESCRIPTION: string;
    JURISDICTION_CODE: string;
    JURISDICTION: string;
    WHEN_CREATED: string;
    WHEN_UPDATED: string;
  }>;
  LEGAL_DESCRIPTION: Partial<{
    BCA_FLD_SYSID: number;
    ROLL_NUMBER: string;
    FOLIO_ID: string;
    FOLIO_STATUS: string;
    FOLIO_STATUS_DESCRIPTION: string;
    LEGAL_DESCRIPTIONS_COUNT: number;
    LEGAL_DESCRIPTION_ID: string;
    BLOCK: string;
    SUB_BLOCK: string;
    DISTRICT_LOT: string;
    EXCEPT_PLAN: string;
    FORMATTED_LEGAL_DESCRIPTION: string;
    LAND_BRANCH_FILE_NUMBER: string;
    LAND_DISTRICT: string;
    LAND_DISTRICT_DESCRIPTION: string;
    LEGAL_TEXT: string;
    LOT: string;
    PID_NUMBER: number;
    PID: string;
    PART_1: string;
    PART_2: string;
    PART_3: string;
    PART_4: string;
    PORTION: string;
    SUB_LOT: string;
    TOWNSHIP: string;
    PLAN: string;
    RANGE: string;
    SECTION: string;
    STRATA_LOT: string;
    LEGAL_SUBDIVISION: string;
    PARCEL: string;
    LEASE_LICENCE_NUMBER: string;
    MERIDIAN: string;
    MERIDIAN_SHORT: string;
    BCA_GROUP: string;
    FIRST_NATION_RESERVE_NUMBER: string;
    FIRST_NATION_RESERVE_DESC: string;
    AIR_SPACE_PARCEL_NUMBER: string;
    JURISDICTION_CODE: string;
    JURISDICTION: string;
    WHEN_CREATED: string;
    WHEN_UPDATE: string;
  }>;
  ADDRESSES: Partial<{
    SE_ANNO_CAD_DATA: string;
    BCA_FA_SYSID: number;
    ROLL_NUMBER: string;
    FOLIO_ID: string;
    FOLIO_STATUS: string;
    FOLIO_STATUS_DESCRIPTION: string;
    ADDRESS_COUNT: number;
    ADDRESS_ID: string;
    UNIT_NUMBER: string;
    STREET_NUMBER: string;
    STREET_DIRECTION_PREFIX: string;
    STREET_NAME: string;
    STREET_TYPE: string;
    STREET_DIRECTION_SUFFIX: string;
    CITY: string;
    POSTAL_CODE: string;
    PROVINCE: string;
    PRIMARY_IND: string;
    MAP_REFERENCE_NUMBER: string;
    JURISDICTION_CODE: string;
    JURISDICTION: string;
    WHEN_CREATED: string;
    WHEN_UPDATED: string;
    EXPIRY_DATE: string;
    FEATURE_AREA_SQM: number;
    FEATURE_LENGTH_M: number;
    SHAPE: any;
    OBJECTID: number;
  }>[];
  SALES: Partial<{
    BCA_FS_SYSID: number;
    ROLL_NUMBER: string;
    FOLIO_ID: string;
    FOLIO_STATUS: string;
    FOLIO_STATUS_DESCRIPTION: string;
    SALES_COUNT: number;
    SALES_ID: string;
    DOCUMENT_NUMBER: string;
    CONVEYANCE_DATE: string;
    CONVEYANCE_PRICE: number;
    CONVEYANCE_TYPE: string;
    CONVEYANCE_TYPE_DESCRIPTION: string;
    JURISDICTION_CODE: string;
    JURISDICTION: string;
    WHEN_CREATED: string;
    WHEN_UPDATED: string;
    EXPIRY_DATE: string;
    FEATURE_AREA_SQM: number;
    FEATURE_LENGTH_M: number;
    SHAPE: any;
    OBJECTID: number;
    SE_ANNO_CAD_DATA: string;
  }>[];
  VALUES: Partial<{
    BCA_FGPV_SYSID: number;
    ROLL_NUMBER: string;
    FOLIO_ID: string;
    FOLIO_STATUS: string;
    FOLIO_STATUS_DESCRIPTION: string;
    GEN_VALUES_COUNT: number;
    GEN_GROSS_IMPROVEMENT_VALUE: number;
    GEN_GROSS_LAND_VALUE: number;
    GEN_NET_IMPROVEMENT_VALUE: number;
    GEN_NET_LAND_VALUE: number;
    GEN_TXXMT_IMPROVEMENT_VALUE: number;
    GEN_TXXMT_LAND_VALUE: number;
    GEN_PROPERTY_CLASS_CODE: string;
    GEN_PROPERTY_CLASS_DESC: string;
    GEN_PROPERTY_SUBCLASS_CODE: string;
    GEN_PROPERTY_SUBCLASS_DESC: string;
    JURISDICTION_CODE: string;
    JURISDICTION: string;
    WHEN_CREATED: string;
    WHEN_UPDATE: string;
  }>[];
  CHARGES: Partial<{
    SE_ANNO_CAD_DATA: string;
    BCA_FLC_SYSID: number;
    ROLL_NUMBER: string;
    FOLIO_ID: string;
    FOLIO_STATUS: string;
    FOLIO_STATUS_DESCRIPTION: string;
    LAND_CHARACTERISTICS_COUNT: number;
    LAND_CHARACTERISTIC_CODE: string;
    LAND_CHARACTERISTIC_DESC: string;
    JURISDICTION_CODE: string;
    JURISDICTION: string;
    WHEN_CREATED: string;
    WHEN_UPDATED: string;
    EXPIRY_DATE: string;
    FEATURE_AREA_SQM: number;
    FEATURE_LENGTH_M: number;
    SHAPE: any;
    OBJECTID: number;
  }>[];
}
