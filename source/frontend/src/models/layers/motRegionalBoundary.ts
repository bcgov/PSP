// Ministry of Transportation (MOT) Regional Boundary
// Source : https://catalogue.data.gov.bc.ca/dataset/ministry-of-transportation-mot-regional-boundary
// Api: WHSE_ADMIN_BOUNDARIES.TADM_MOT_REGIONAL_BNDRY_POLY
export interface MOT_RegionalBoundary_Feature_Properties {
  readonly REGION_NUMBER: number | null;
  readonly REGION_NAME: string | null;
  readonly OBJECTID: number | null;
}

export const emptyRegion: MOT_RegionalBoundary_Feature_Properties = {
  REGION_NUMBER: null,
  REGION_NAME: null,
  OBJECTID: null,
};
