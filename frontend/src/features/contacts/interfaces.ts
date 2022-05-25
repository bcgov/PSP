export enum ContactTypes {
  ORGANIZATION = 'O',
  INDIVIDUAL = 'P',
}

export interface ContactInfoField {
  info: string;
  label: string;
}

export interface AddressField {
  label: string;
  streetAddress1?: string;
  streetAddress2?: string;
  streetAddress3?: string;
  municipalityAndProvince?: string;
  country?: string;
  postal?: string;
}
