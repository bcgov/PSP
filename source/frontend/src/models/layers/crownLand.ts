import { Geometry } from 'geojson';

import { UtcIsoDate } from '../api/UtcIsoDateTime';

// TANTALIS feature properties
// For more information see: https://catalogue.data.gov.bc.ca/dataset/

// TANTALIS - Crown Land Leases
// Source : https://openmaps.gov.bc.ca/geo/pub/WHSE_TANTALIS.TA_CROWN_LEASES_SVW/
// Api: WHSE_TANTALIS.TA_CROWN_LEASES_SVW'
export interface TANTALIS_CrownLandLeases_Feature_Properties {
  readonly INTRID_SID: number | null;
  readonly DISPOSITION_TRANSACTION_SID: number | null;
  readonly TENURE_STAGE: string | null;
  readonly TENURE_STATUS: string | null;
  readonly TENURE_TYPE: string | null;
  readonly TENURE_SUBTYPE: string | null;
  readonly TENURE_PURPOSE: string | null;
  readonly TENURE_SUBPURPOSE: string | null;
  readonly CROWN_LANDS_FILE: string | null;
  readonly TENURE_DOCUMENT: string | null;
  readonly TENURE_EXPIRY: UtcIsoDate | null;
  readonly TENURE_LOCATION: string | null;
  readonly TENURE_LEGAL_DESCRIPTION: string | null;
  readonly TENURE_AREA_DERIVATION: string | null;
  readonly TENURE_AREA_IN_HECTARES: number | null;
  readonly RESPONSIBLE_BUSINESS_UNIT: string | null;
  readonly CODE_CHR_STAGE: string | null;
  readonly FEATURE_CODE: string | null;
  //readonly SHAPE: SDO_GEOMETRY | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
}

// TANTALIS - Crown Land Inventory
// Source : https://openmaps.gov.bc.ca/geo/pub/WHSE_TANTALIS.TA_CROWN_INVENTORY_SVW
// Api: WHSE_TANTALIS.TA_CROWN_INVENTORY_SVW
export interface TANTALIS_CrownLandInventory_Feature_Properties {
  readonly INTRID_SID: number | null;
  readonly TENURE_STAGE: string | null;
  readonly TENURE_STATUS: string | null;
  readonly TENURE_TYPE: string | null;
  readonly TENURE_SUBTYPE: string | null;
  readonly TENURE_PURPOSE: string | null;
  readonly TENURE_SUBPURPOSE: string | null;
  readonly CROWN_LANDS_FILE: string | null;
  readonly TENURE_DOCUMENT: string | null;
  readonly TENURE_EXPIRY: UtcIsoDate | null;
  readonly TENURE_LOCATION: string | null;
  readonly TENURE_LEGAL_DESCRIPTION: string | null;
  readonly TENURE_AREA_DERIVATION: string | null;
  readonly TENURE_AREA_IN_HECTARES: number | null;
  readonly RESPONSIBLE_BUSINESS_UNIT: string | null;
  readonly DISPOSITION_TRANSACTION_SID: number | null;
  readonly CODE_CHR_STAGE: string | null;
  readonly FEATURE_CODE: string | null;
  //readonly SHAPE:	SDO_GEOMETRY | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
}

// TANTALIS - Crown Land Licenses
// Source : https://openmaps.gov.bc.ca/geo/pub/WHSE_TANTALIS.TA_CROWN_LICENSES_SVW
// Api: WHSE_TANTALIS.TA_CROWN_LICENSES_SVW
export interface TANTALIS_CrownLandLicenses_Feature_Properties {
  readonly INTRID_SID: number | null;
  readonly TENURE_STAGE: string | null;
  readonly TENURE_STATUS: string | null;
  readonly TENURE_TYPE: string | null;
  readonly TENURE_SUBTYPE: string | null;
  readonly TENURE_PURPOSE: string | null;
  readonly TENURE_SUBPURPOSE: string | null;
  readonly CROWN_LANDS_FILE: string | null;
  readonly TENURE_DOCUMENT: string | null;
  readonly TENURE_EXPIRY: UtcIsoDate | null;
  readonly TENURE_LOCATION: string | null;
  readonly TENURE_LEGAL_DESCRIPTION: string | null;
  readonly TENURE_AREA_DERIVATION: string | null;
  readonly TENURE_AREA_IN_HECTARES: number | null;
  readonly RESPONSIBLE_BUSINESS_UNIT: string | null;
  readonly DISPOSITION_TRANSACTION_SID: number | null;
  readonly CODE_CHR_STAGE: string | null;
  readonly FEATURE_CODE: string | null;
  //readonly SHAPE: SDO_GEOMETRY | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
}

// TANTALIS - Crown Land Tenures
// Source : https://openmaps.gov.bc.ca/geo/pub/WHSE_TANTALIS.TA_CROWN_TENURES_SVW
// Api: WHSE_TANTALIS.TA_CROWN_TENURES_SVW
export interface TANTALIS_CrownLandTenures_Feature_Properties {
  readonly INTRID_SID: number | null;
  readonly TENURE_STAGE: string | null;
  readonly TENURE_STATUS: string | null;
  readonly TENURE_TYPE: string | null;
  readonly TENURE_SUBTYPE: string | null;
  readonly TENURE_PURPOSE: string | null;
  readonly TENURE_SUBPURPOSE: string | null;
  readonly CROWN_LANDS_FILE: string | null;
  readonly APPLICATION_TYPE_CDE: string | null;
  readonly TENURE_DOCUMENT: string | null;
  readonly TENURE_EXPIRY: UtcIsoDate | null;
  readonly TENURE_LOCATION: string | null;
  readonly TENURE_LEGAL_DESCRIPTION: string | null;
  readonly TENURE_AREA_DERIVATION: string | null;
  readonly TENURE_AREA_IN_HECTARES: number | null;
  readonly RESPONSIBLE_BUSINESS_UNIT: string | null;
  readonly DISPOSITION_TRANSACTION_SID: number | null;
  readonly CODE_CHR_STAGE: string | null;
  readonly FEATURE_CODE: string | null;
  //readonly SHAPE: SDO_GEOMETRY| null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
}

// TANTALIS - Crown Land Inclusions
// Source : https://openmaps.gov.bc.ca/geo/pub/WHSE_TANTALIS.TA_CROWN_INCLUSIONS_SVW
// Api: WHSE_TANTALIS.TA_CROWN_INCLUSIONS_SVW
export interface TANTALIS_CrownLandInclusions_Feature_Properties {
  readonly INTRID_SID: number | null;
  readonly TENURE_STAGE: string | null;
  readonly TENURE_STATUS: string | null;
  readonly TENURE_TYPE: string | null;
  readonly TENURE_SUBTYPE: string | null;
  readonly TENURE_PURPOSE: string | null;
  readonly TENURE_SUBPURPOSE: string | null;
  readonly CROWN_LANDS_FILE: string | null;
  readonly TENURE_DOCUMENT: string | null;
  readonly TENURE_EXPIRY: UtcIsoDate | null;
  readonly TENURE_LOCATION: string | null;
  readonly TENURE_LEGAL_DESCRIPTION: string | null;
  readonly TENURE_AREA_DERIVATION: string | null;
  readonly TENURE_AREA_IN_HECTARES: number | null;
  readonly RESPONSIBLE_BUSINESS_UNIT: string | null;
  readonly DISPOSITION_TRANSACTION_SID: number | null;
  readonly CODE_CHR_STAGE: string | null;
  readonly FEATURE_CODE: string | null;
  //readonly SHAPE: SDO_GEOMETRY | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
}

// TANTALIS - Crown Surveyed Parcels
// Source : https://openmaps.gov.bc.ca/geo/pub/WHSE_TANTALIS.TA_SURVEY_PARCELS_SVW
// Api: WHSE_TANTALIS.TA_SURVEY_PARCELS_SVW
export interface TANTALIS_CrownSurveyParcels_Feature_Properties {
  readonly PARCEL_FABRIC_POLY_ID: number | null;
  readonly GLOBAL_UID: string | null;
  readonly PARCEL_NAME: string | null;
  readonly PLAN_ID: string | null;
  readonly PLAN_NUMBER: string | null;
  readonly PIN: number | null;
  readonly PID: string | null;
  readonly PID_FORMATTED: string | null;
  readonly PID_NUMBER: number | null;
  readonly SOURCE_PARCEL_ID: string | null;
  readonly PARCEL_STATUS: string | null;
  readonly PARCEL_CLASS: string | null;
  readonly OWNER_TYPE: string | null;
  readonly PARCEL_START_DATE: UtcIsoDate | null;
  readonly SURVEY_DESIGNATION_1: string | null;
  readonly SURVEY_DESIGNATION_2: string | null;
  readonly SURVEY_DESIGNATION_3: string | null;
  readonly LEGAL_DESCRIPTION: string | null;
  readonly MUNICIPALITY: string | null;
  readonly REGIONAL_DISTRICT: string | null;
  readonly IS_REMAINDER_IND: string | null;
  readonly GEOMETRY_SOURCE: string | null;
  readonly POSITIONAL_ERROR: number | null;
  readonly ERROR_REPORTED_BY: string | null;
  readonly CAPTURE_METHOD: string | null;
  readonly COMPILED_IND: string | null;
  readonly STATED_AREA: string | null;
  readonly WHEN_CREATED: UtcIsoDate | null;
  readonly WHEN_UPDATED: UtcIsoDate | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
  readonly SHAPE: Geometry;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
}
