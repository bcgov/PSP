import { AddressTypes } from 'constants/addressTypes';
import { NumberFieldValue } from 'typings/NumberFieldValue';

export interface ICreatePerson {
  id?: number;
  surname: string;
  firstName: string;
  middleNames?: string;
  preferredName?: string;
  comment?: string;
  isDisabled: boolean;
  organizationId?: string;
  addresses?: ICreateContactAddress[];
  contactMethods?: ICreateContactMethod[];
}

export interface ICreateContactAddress {
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

export interface ICreateContactMethod {
  id?: number;
  rowVersion?: number;
  contactMethodTypeCode: string;
  value: string;
}

export interface ICreatePersonForm
  extends ExtendOverride<
    ICreatePerson,
    {
      emailContactMethods: ICreateContactMethod[];
      phoneContactMethods: ICreateContactMethod[];
      mailingAddress: ICreateContactAddressForm;
      propertyAddress: ICreateContactAddressForm;
      billingAddress: ICreateContactAddressForm;
    }
  > {}

export interface ICreateContactAddressForm
  extends ExtendOverride<
    ICreateContactAddress,
    {
      provinceId: NumberFieldValue;
      countryId: NumberFieldValue;
    }
  > {}

export const defaultCreatePerson: ICreatePersonForm = {
  isDisabled: false,
  firstName: '',
  middleNames: '',
  surname: '',
  preferredName: '',
  comment: '',
  emailContactMethods: [
    {
      contactMethodTypeCode: '',
      value: '',
    },
  ],
  phoneContactMethods: [
    {
      contactMethodTypeCode: '',
      value: '',
    },
  ],
  mailingAddress: {
    addressTypeId: AddressTypes.Mailing,
    streetAddress1: '',
    streetAddress2: '',
    streetAddress3: '',
    municipality: '',
    countryOther: '',
    postal: '',
    countryId: '',
    provinceId: '',
  },
  propertyAddress: {
    addressTypeId: AddressTypes.Residential,
    streetAddress1: '',
    streetAddress2: '',
    streetAddress3: '',
    municipality: '',
    countryOther: '',
    postal: '',
    countryId: '',
    provinceId: '',
  },
  billingAddress: {
    addressTypeId: AddressTypes.Billing,
    streetAddress1: '',
    streetAddress2: '',
    streetAddress3: '',
    municipality: '',
    countryOther: '',
    postal: '',
    countryId: '',
    provinceId: '',
  },
};
