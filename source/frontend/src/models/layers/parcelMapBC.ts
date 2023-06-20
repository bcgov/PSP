export type stringDate = string;

// ParcelMap BC Parcel Fabric - Fully Attributed
// Source : https://catalogue.data.gov.bc.ca/dataset/parcelmap-bc-parcel-fabric-fully-attributed
// Api: WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_FA_SVW
export interface PMBC_FullyAttributed_Feature_Properties {
  readonly PARCEL_FABRIC_POLY_ID: number | null;
  readonly GLOBAL_UID: string | null;
  readonly PARCEL_NAME: string | null;
  readonly PLAN_ID: number | null;
  readonly PLAN_NUMBER: string | null;
  readonly PIN: number | null;
  readonly PID: string | null;
  readonly PID_FORMATTED: string | null;
  readonly PID_NUMBER: number | null;
  readonly SOURCE_PARCEL_ID: string | null;
  readonly PARCEL_STATUS: string | null;
  readonly PARCEL_CLASS: string | null;
  readonly OWNER_TYPE: string | null;
  readonly PARCEL_START_DATE: stringDate | null;
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
  readonly WHEN_CREATED: stringDate | null;
  readonly WHEN_UPDATED: stringDate | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
  //readonly SHAPE: sdo_geometry | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
}
