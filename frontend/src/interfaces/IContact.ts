import { AddressTypes } from 'constants/addressTypes';

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

export interface IContactPersonCreate {
  id?: number;
  surname: string;
  firstName: string;
  middleNames?: string;
  preferredName?: string;
  comment?: string;
  isDisabled: boolean;
  organizationId?: string;
  addresses: IContactAddressCreate[];
  contactMethods: IContactMethodCreate[];
}

export interface IContactAddressCreate {
  id?: number;
  rowVersion?: number;
  addressType: string;
  streetAddress1: string;
  streetAddress2?: string;
  streetAddress3?: string;
  regionId?: number;
  districtId?: number;
  municipality?: string;
  provinceId?: number;
  countryId?: number;
  countryOther?: string;
  postal?: string;
}

export interface IContactMethodCreate {
  id?: number;
  rowVersion?: number;
  contactMethodType: string;
  value: string;
}

export const defaultCreatePerson: IContactPersonCreate = {
  isDisabled: false,
  firstName: '',
  middleNames: '',
  surname: '',
  preferredName: '',
  comment: '',
  contactMethods: [],
  addresses: [
    {
      addressType: AddressTypes.Mailing,
      streetAddress1: '',
      streetAddress2: '',
      streetAddress3: '',
      municipality: '',
      countryOther: '',
      postal: '',
    },
    {
      addressType: AddressTypes.Mailing,
      streetAddress1: '',
      streetAddress2: '',
      streetAddress3: '',
      municipality: '',
      countryOther: '',
      postal: '',
    },
    {
      addressType: AddressTypes.Mailing,
      streetAddress1: '',
      streetAddress2: '',
      streetAddress3: '',
      municipality: '',
      countryOther: '',
      postal: '',
    },
  ],
};
