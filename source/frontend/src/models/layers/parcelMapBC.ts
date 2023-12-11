export type stringDate = string;

// ParcelMap BC Parcel Fabric
// Source : https://catalogue.data.gov.bc.ca/dataset/parcelmap-bc-parcel-fabric
// Api: WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_FA_SVW
export interface PMBC_Feature_Properties {
  readonly PARCEL_FABRIC_POLY_ID: number | null;
  readonly GLOBAL_UID: string | null;
  readonly PARCEL_NAME: string | null;
  readonly PLAN_NUMBER: string | null;
  readonly PIN: number | null;
  readonly PID: string | null;
  readonly PID_FORMATTED: string | null;
  readonly PID_NUMBER: number | null;
  readonly PARCEL_STATUS: string | null;
  readonly PARCEL_CLASS: string | null;
  readonly OWNER_TYPE: string | null;
  readonly PARCEL_START_DATE: stringDate | null;
  readonly MUNICIPALITY: string | null;
  readonly REGIONAL_DISTRICT: string | null;
  readonly WHEN_UPDATED: stringDate | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
  //readonly SHAPE: sdo_geometry | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
}

export const emptyPmbcParcel: PMBC_Feature_Properties = {
  PARCEL_FABRIC_POLY_ID: null,
  GLOBAL_UID: null,
  PARCEL_NAME: null,
  PLAN_NUMBER: null,
  PIN: null,
  PID: null,
  PID_FORMATTED: null,
  PID_NUMBER: null,
  PARCEL_STATUS: null,
  PARCEL_CLASS: null,
  OWNER_TYPE: null,
  PARCEL_START_DATE: null,
  MUNICIPALITY: null,
  REGIONAL_DISTRICT: null,
  WHEN_UPDATED: null,
  FEATURE_AREA_SQM: null,
  FEATURE_LENGTH_M: null,

  OBJECTID: null,
  SE_ANNO_CAD_DATA: null,
};
