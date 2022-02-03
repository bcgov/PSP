import { AddressTypes } from 'constants/addressTypes';
import { NumberFieldValue } from 'typings/NumberFieldValue';

import ITypeCode from '../ITypeCode';

export interface IEditablePerson {
  id?: number;
  rowVersion?: number;
  surname: string;
  firstName: string;
  middleNames?: string;
  preferredName?: string;
  comment?: string;
  isDisabled: boolean;
  organization?: IOrganizationLink;
  personOrganizationId?: number;
  personOrganizationRowVersion?: number;
  addresses?: IEditablePersonAddress[];
  contactMethods?: IEditableContactMethod[];
}

export interface ICreateOrganization {
  id?: number;
  name: string;
  alias?: string;
  incorporationNumber?: string;
  comment?: string;
  isDisabled: boolean;
  addresses?: IEditableOrganizationAddress[];
  contactMethods?: IEditableContactMethod[];
}

// internal interface - not meant to be imported/shared
interface IBaseAddress {
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

export interface IEditablePersonAddress extends IBaseAddress {
  personId?: number;
  personAddressId?: number;
  personAddressRowVersion?: number;
}

export interface IEditableOrganizationAddress extends IBaseAddress {
  organizationId?: number;
  organizationAddressId?: number;
  organizationAddressRowVersion?: number;
}

export interface IEditableContactMethod {
  id?: number;
  rowVersion?: number;
  personId?: number;
  organizationId?: number;
  contactMethodTypeCode: ITypeCode<string>;
  value: string;
}

export interface IOrganizationLink {
  id: number;
  text: string;
}

export interface IEditablePersonForm
  extends ExtendOverride<
    IEditablePerson,
    {
      isDisabled: string | boolean;
      emailContactMethods: IEditableContactMethodForm[];
      phoneContactMethods: IEditableContactMethodForm[];
      mailingAddress: IEditablePersonAddressForm;
      propertyAddress: IEditablePersonAddressForm;
      billingAddress: IEditablePersonAddressForm;
    }
  > {}

export interface ICreateOrganizationForm
  extends ExtendOverride<
    ICreateOrganization,
    {
      emailContactMethods: IEditableContactMethodForm[];
      phoneContactMethods: IEditableContactMethodForm[];
      mailingAddress: IEditableOrganizationAddressForm;
      propertyAddress: IEditableOrganizationAddressForm;
      billingAddress: IEditableOrganizationAddressForm;
    }
  > {}

export interface IEditableContactMethodForm
  extends ExtendOverride<IEditableContactMethod, { contactMethodTypeCode: string }> {}

export interface IEditablePersonAddressForm
  extends ExtendOverride<
    IEditablePersonAddress,
    { countryId: NumberFieldValue; provinceId: NumberFieldValue; addressTypeId: string }
  > {}

export interface IEditableOrganizationAddressForm
  extends ExtendOverride<
    IEditableOrganizationAddress,
    { countryId: NumberFieldValue; provinceId: NumberFieldValue; addressTypeId: string }
  > {}

export const getDefaultAddress = (addressType: AddressTypes) =>
  ({
    addressTypeId: addressType,
    streetAddress1: '',
    streetAddress2: '',
    streetAddress3: '',
    municipality: '',
    countryOther: '',
    postal: '',
    countryId: '',
    provinceId: '',
  } as IEditablePersonAddressForm | IEditableOrganizationAddressForm);

export const getDefaultContactMethod = () =>
  ({
    contactMethodTypeCode: '',
    value: '',
  } as IEditableContactMethodForm);

export const defaultCreatePerson: IEditablePersonForm = {
  isDisabled: false,
  firstName: '',
  middleNames: '',
  surname: '',
  preferredName: '',
  comment: '',
  organization: undefined,
  emailContactMethods: [getDefaultContactMethod()],
  phoneContactMethods: [getDefaultContactMethod()],
  mailingAddress: getDefaultAddress(AddressTypes.Mailing),
  propertyAddress: getDefaultAddress(AddressTypes.Residential),
  billingAddress: getDefaultAddress(AddressTypes.Billing),
};

export const defaultCreateOrganization: ICreateOrganizationForm = {
  isDisabled: false,
  name: '',
  alias: '',
  incorporationNumber: '',
  comment: '',
  emailContactMethods: [getDefaultContactMethod()],
  phoneContactMethods: [getDefaultContactMethod()],
  mailingAddress: getDefaultAddress(AddressTypes.Mailing),
  propertyAddress: getDefaultAddress(AddressTypes.Residential),
  billingAddress: getDefaultAddress(AddressTypes.Billing),
};
