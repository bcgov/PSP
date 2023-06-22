export type stringDate = string;

// Municipalities - Legally Defined Administrative Areas of BC
// Source : https://catalogue.data.gov.bc.ca/dataset/municipalities-legally-defined-administrative-areas-of-bc
// Api: WHSE_LEGAL_ADMIN_BOUNDARIES.ABMS_MUNICIPALITIES_SP
export interface WHSE_Municipalities_Feature_Properties {
  readonly LGL_ADMIN_AREA_ID: number | null;
  readonly ADMIN_AREA_NAME: string | null;
  readonly ADMIN_AREA_ABBREVIATION: string | null;
  readonly ADMIN_AREA_BOUNDARY_TYPE: string | null;
  readonly ADMIN_AREA_GROUP_NAME: string | null;
  readonly CHANGE_REQUESTED_ORG: string | null;
  readonly UPDATE_TYPE: string | null;
  readonly WHEN_UPDATED: stringDate | null;
  readonly MAP_STATUS: string | null;
  readonly OIC_MO_NUMBER: string | null;
  readonly OIC_MO_YEAR: string | null;
  readonly OIC_MO_TYPE: string | null;
  readonly WEBSITE_URL: string | null;
  readonly IMAGE_URL: string | null;
  readonly AFFECTED_ADMIN_AREA_ABRVN: string | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
  //readonly SHAPE: SDO_GEOMETRY | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
}
