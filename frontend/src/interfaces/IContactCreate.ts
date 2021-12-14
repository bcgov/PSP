import { AddressTypes } from 'constants/addressTypes';

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
  addressTypeId: string;
  streetAddress1: string;
  streetAddress2?: string;
  streetAddress3?: string;
  municipality?: string;
  regionId?: number;
  districtId?: number;
  provinceId?: number;
  countryId?: number;
  countryOther?: string;
  postal?: string;
}

export interface IContactMethodCreate {
  id?: number;
  rowVersion?: number;
  contactMethodTypeCode: string;
  value: string;
}

export const defaultCreatePerson: IContactPersonCreate = {
  isDisabled: false,
  firstName: '',
  middleNames: '',
  surname: '',
  preferredName: '',
  comment: '',
  contactMethods: [
    {
      contactMethodTypeCode: '',
      value: '',
    },
  ],
  addresses: [
    {
      addressTypeId: AddressTypes.Mailing,
      streetAddress1: '',
      streetAddress2: '',
      streetAddress3: '',
      municipality: '',
      countryOther: '',
      postal: '',
    },
    {
      addressTypeId: AddressTypes.Residential,
      streetAddress1: '',
      streetAddress2: '',
      streetAddress3: '',
      municipality: '',
      countryOther: '',
      postal: '',
    },
    {
      addressTypeId: AddressTypes.Billing,
      streetAddress1: '',
      streetAddress2: '',
      streetAddress3: '',
      municipality: '',
      countryOther: '',
      postal: '',
    },
  ],
};
