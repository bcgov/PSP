export type stringDate = string;

// ALC Agricultural Land Reserve Lines
// Source : https://catalogue.data.gov.bc.ca/dataset/alc-agricultural-land-reserve-lines
// Api: WHSE_LEGAL_ADMIN_BOUNDARIES.OATS_ALR_BOUNDARY_LINES_SVW
export interface WHSE_AgriculturalLandReserve_Feature_Properties {
  readonly ALR_LINE_ID: number | null;
  readonly BOUNDARY_TYPE: string | null;
  readonly FEATURE_CODE: string | null;
  //readonly GEOMETRY: SDO_GEOMETRY | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
  readonly FEATURE_LENGTH_M: number | null;
}
