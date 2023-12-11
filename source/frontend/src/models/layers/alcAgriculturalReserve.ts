export type stringDate = string;

// ALC Agricultural Land Reserve Lines
// Source : https://catalogue.data.gov.bc.ca/dataset/alc-agricultural-land-reserve-lines
// Api: WHSE_LEGAL_ADMIN_BOUNDARIES.OATS_ALR_BOUNDARY_LINES_SVW
export interface WHSE_AgriculturalLandReserveLine_Feature_Properties {
  readonly ALR_LINE_ID: number | null;
  readonly BOUNDARY_TYPE: string | null;
  readonly FEATURE_CODE: string | null;
  //readonly GEOMETRY: SDO_GEOMETRY | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
  readonly FEATURE_LENGTH_M: number | null;
}

// ALC ALR Polygons
// Source : https://catalogue.data.gov.bc.ca/dataset/alc-alr-polygons
// Api: WHSE_LEGAL_ADMIN_BOUNDARIES.OATS_ALR_POLYS
export interface WHSE_AgriculturalLandReservePoly_Feature_Properties {
  readonly ALR_POLY_ID: number | null;
  readonly STATUS: string | null;
  readonly FEATURE_CODE: string | null;
  // readonly GEOMETRY	: SDO_GEOMETRNumber | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
}
