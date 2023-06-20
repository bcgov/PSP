// Ministry of Transportation (MOT) District Boundary
// Source : https://catalogue.data.gov.bc.ca/dataset/ministry-of-transportation-mot-district-boundary
// Api: WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY
export interface MOT_DistrictBoundary_Feature_Properties {
  readonly DISTRICT_NUMBER: number | null;
  readonly DISTRICT_NAME: string | null;
  readonly FEATURE_CODE: string | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
}
