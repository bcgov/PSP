export type stringDate = string;

// Indian Reserves and Band Names - Administrative Boundaries
// Source : https://catalogue.data.gov.bc.ca/dataset/indian-reserves-and-band-names-administrative-boundaries
// Api: WHSE_ADMIN_BOUNDARIES.ADM_INDIAN_RESERVES_BANDS_SP
export interface ADM_IndianReserveBands_Feature_Properties {
  readonly CLAB_ID: string | null;
  readonly ENGLISH_NAME: string | null;
  readonly FRENCH_NAME: string | null;
  readonly ABSOLUTE_ACCURACY: string | null;
  readonly BAND_NUMBER: string | null;
  readonly BAND_NAME: string | null;
  //readonly GEOMETRY: SDO_GEOMETRY | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
}
