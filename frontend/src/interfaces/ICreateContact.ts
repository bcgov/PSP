import { AddressTypes } from 'constants/addressTypes';
import { NumberFieldValue } from 'typings/NumberFieldValue';

import ITypeCode from './ITypeCode';

export interface ICreatePerson {
  id?: number;
  surname: string;
  firstName: string;
  middleNames?: string;
  preferredName?: string;
  comment?: string;
  isDisabled: boolean;
  organizationId?: number;
  addresses?: ICreateContactAddress[];
  contactMethods?: ICreateContactMethod[];
}

export interface ICreateContactAddress {
  id?: number;
  rowVersion?: number;
  addressTypeId: ITypeCode<string>;
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
  contactMethodTypeCode: ITypeCode<string>;
  value: string;
}

export interface ICreatePersonForm
  extends ExtendOverride<
    ICreatePerson,
    {
      organizationId: NumberFieldValue;
      emailContactMethods: ICreateContactMethodForm[];
      phoneContactMethods: ICreateContactMethodForm[];
      mailingAddress: ICreateContactAddressForm;
      propertyAddress: ICreateContactAddressForm;
      billingAddress: ICreateContactAddressForm;
    }
  > {}

export interface ICreateContactMethodForm
  extends ExtendOverride<ICreateContactMethod, { contactMethodTypeCode: string }> {}

export interface ICreateContactAddressForm
  extends ExtendOverride<
    ICreateContactAddress,
    { countryId: NumberFieldValue; provinceId: NumberFieldValue; addressTypeId: string }
  > {}

export const defaultCreatePerson: ICreatePersonForm = {
  isDisabled: false,
  firstName: '',
  middleNames: '',
  surname: '',
  preferredName: '',
  comment: '',
  organizationId: '',
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
