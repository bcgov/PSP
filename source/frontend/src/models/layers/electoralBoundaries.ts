export type stringDate = string;

// Provincial Electoral Districts - Electoral Boundaries Redistribution, 2015
// Source : https://catalogue.data.gov.bc.ca/dataset/provincial-electoral-districts-electoral-boundaries-redistribution-2015
// Api: WHSE_ADMIN_BOUNDARIES.EBC_ELECTORAL_DISTS_BS10_SVW
export interface EBC_ELECTORAL_DISTS_BS10_SVW_Feature_Properties {
  readonly ELECTORAL_DISTRICT_ID: number | null;
  readonly BOUNDARY_SET_ID: number | null;
  readonly ED_ABBREVIATION: string | null;
  readonly ED_NAME: string | null;
  readonly GAZETTE_DATE: stringDate | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
  //readonly SHAPE: SDO_GEOMETRY | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
}
