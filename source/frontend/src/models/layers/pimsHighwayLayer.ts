export type stringDate = string;

// PIMS Highway layer
// Source : N/A
// Api: ? ISS_PROVINCIAL_PUBLIC_HIGHWAY
export interface ISS_ProvincialPublicHighway {
  readonly PROVINCIAL_PUBLIC_HIGHWAY_ID: number | null;
  readonly GLOBALID: string | null;
  readonly UNIQUE_ID: string | null;
  readonly PLAN_ANNOTATION: string | null;
  readonly MOTI_PLAN: string | null;
  readonly VETTING_STATUS: string | null;
  readonly SHAPE_TYPE: string | null;
  readonly PENDING_CLASSIFICATION: string | null;
  readonly MOTI_FILE: string | null;
  readonly GAZETTE_PUBLISHED_DATE: stringDate | null;
  readonly GAZETTE_PUBLISHED_ON: string | null;
  readonly PROV_PUBLIC_HIGHWAY_TYPE: string | null;
  readonly ORDER_IN_COUNCIL: string | null;
  readonly SHORT_LEGAL_DESCRIPTION: string | null;
  readonly SURVEY_PLAN: string | null;
  readonly PARENT_PARCEL: string | null;
  readonly PARCELMAPBC_COMMENT: string | null;
  readonly CREATE_TIME: stringDate | null;
  readonly UPDATE_TIME: stringDate | null;
  readonly CREATE_USER: string | null;
  readonly UPDATE_USER: string | null;
  readonly LTSA_CREATE_TIME: stringDate | null;
  readonly LTSA_UPDATE_TIME: stringDate | null;
  readonly LTSA_CREATE_USER: string | null;
  readonly LTSA_UPDATE_USER: string | null;
  readonly RELATED_GAZETTES: string | null;
  readonly HYPERLINK: string | null;
  readonly RELATIVE_PATH: string | null;
}
