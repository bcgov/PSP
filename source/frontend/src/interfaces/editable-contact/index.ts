import { AddressTypes } from '@/constants/addressTypes';
import { IContactPerson } from '@/interfaces/IContact';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { NumberFieldValue } from '@/typings/NumberFieldValue';

export interface IEditablePerson {
  id?: number;
  rowVersion?: number;
  surname: string;
  firstName: string;
  middleNames?: string;
  preferredName?: string;
  comment?: string;
  isDisabled: boolean;
  organization: IOrganizationLink | null;
  useOrganizationAddress: boolean;
  personOrganizationId?: number;
  personOrganizationRowVersion?: number;
  addresses?: IEditablePersonAddress[];
  contactMethods?: IEditableContactMethod[];
}

export interface IEditableOrganization {
  id?: number;
  rowVersion?: number;
  name: string;
  alias?: string;
  incorporationNumber?: string;
  comment?: string;
  isDisabled: boolean;
  persons?: Partial<IEditablePerson>[];
  addresses?: IEditableOrganizationAddress[];
  contactMethods?: IEditableContactMethod[];
}

// internal interface - not meant to be imported/shared
export interface IBaseAddress {
  id?: number;
  rowVersion?: number;
  addressTypeId: ApiGen_Base_CodeType<string>;
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
  contactMethodTypeCode: ApiGen_Base_CodeType<string>;
  value: string;
}

export interface IOrganizationLink {
  id: number;
  text: string;
}

export type IEditablePersonForm = ExtendOverride<
  IEditablePerson,
  {
    isDisabled: string | boolean;
    emailContactMethods: IEditableContactMethodForm[];
    phoneContactMethods: IEditableContactMethodForm[];
    mailingAddress: IEditablePersonAddressForm;
    propertyAddress: IEditablePersonAddressForm;
    billingAddress: IEditablePersonAddressForm;
  }
>;

export type IEditableOrganizationForm = ExtendOverride<
  IEditableOrganization,
  {
    persons: Partial<IContactPerson>[];
    emailContactMethods: IEditableContactMethodForm[];
    phoneContactMethods: IEditableContactMethodForm[];
    mailingAddress: IEditableOrganizationAddressForm;
    propertyAddress: IEditableOrganizationAddressForm;
    billingAddress: IEditableOrganizationAddressForm;
  }
>;

export type IEditableContactMethodForm = ExtendOverride<
  IEditableContactMethod,
  { contactMethodTypeCode: string }
>;

export type IEditablePersonAddressForm = ExtendOverride<
  IEditablePersonAddress,
  { countryId: NumberFieldValue; provinceId: NumberFieldValue; addressTypeId: string }
>;

export type IEditableOrganizationAddressForm = ExtendOverride<
  IEditableOrganizationAddress,
  { countryId: NumberFieldValue; provinceId: NumberFieldValue; addressTypeId: string }
>;

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
  organization: null,
  useOrganizationAddress: false,
  emailContactMethods: [getDefaultContactMethod()],
  phoneContactMethods: [getDefaultContactMethod()],
  mailingAddress: getDefaultAddress(AddressTypes.Mailing),
  propertyAddress: getDefaultAddress(AddressTypes.Residential),
  billingAddress: getDefaultAddress(AddressTypes.Billing),
};

export const defaultCreateOrganization: IEditableOrganizationForm = {
  isDisabled: false,
  name: '',
  alias: '',
  incorporationNumber: '',
  comment: '',
  persons: [],
  emailContactMethods: [getDefaultContactMethod()],
  phoneContactMethods: [getDefaultContactMethod()],
  mailingAddress: getDefaultAddress(AddressTypes.Mailing),
  propertyAddress: getDefaultAddress(AddressTypes.Residential),
  billingAddress: getDefaultAddress(AddressTypes.Billing),
};
