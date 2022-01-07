import ITypeCode from './ITypeCode';

export interface IContactCountry {
  countryId: number;
  countryCode: string;
  description: string;
}

export interface IContactProvince {
  provinceStateId: number;
  provinceStateCode: string;
  description: string;
}

export interface IContactAddress {
  id: number;
  rowVersion: number;
  addressType: ITypeCode<string>;
  streetAddress1?: string;
  streetAddress2?: string;
  streetAddress3?: string;
  municipality?: string;
  province: IContactProvince;
  country?: IContactCountry;
  postal?: string;
}

export interface IContactMethod {
  id: number;
  rowVersion: number;
  contactMethodType: ITypeCode<string>;
  value: string;
}

export interface IContactPerson {
  id: number;
  isDisabled: boolean;
  fullName: string;
  preferredName: string;
  organizations?: IContactOrganization[];
  addresses?: IContactAddress[];
  contactMethods?: IContactMethod[];
  comment: string;
}

export interface IContactOrganization {
  id: string;
  isDisabled: boolean;
  name: string;
  alias: string;
  incorporationNumber: string;
  persons?: IContactPerson[];
  addresses?: IContactAddress[];
  contactMethods?: IContactMethod[];
  comment: string;
}

export interface IContact {
  id: string;
  person?: IContactPerson;
  organization?: IContactOrganization;
}
