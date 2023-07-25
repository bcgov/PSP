import { isEmpty } from 'lodash';

import { EmailContactMethods, PhoneContactMethods } from '@/constants/contactMethodType';
import { AddressTypes } from '@/constants/index';
import {
  getDefaultAddress,
  getDefaultContactMethod,
  IBaseAddress,
  IEditableContactMethod,
  IEditableContactMethodForm,
  IEditableOrganization,
  IEditableOrganizationAddress,
  IEditableOrganizationAddressForm,
  IEditableOrganizationForm,
  IEditablePerson,
  IEditablePersonAddress,
  IEditablePersonAddressForm,
  IEditablePersonForm,
} from '@/interfaces/editable-contact';
import { IContactPerson } from '@/interfaces/IContact';
import { Api_Organization, Api_OrganizationPerson } from '@/models/api/Organization';
import { fromTypeCode, stringToBoolean, stringToUndefined, toTypeCode } from '@/utils/formUtils';
import { formatFullName } from '@/utils/personUtils';

import { Api_Address } from './../../models/api/Address';
import { Api_Person } from './../../models/api/Person';

export function formPersonToApiPerson(formValues: IEditablePersonForm): IEditablePerson {
  // exclude form-specific fields from API payload object
  const {
    mailingAddress,
    propertyAddress,
    billingAddress,
    emailContactMethods,
    phoneContactMethods,
    ...restObject
  } = formValues;

  const addresses = [mailingAddress, propertyAddress, billingAddress]
    .filter(hasAddress)
    .map(formAddressToApiAddress);

  const contactMethods = [...emailContactMethods, ...phoneContactMethods]
    .filter(hasContactMethod)
    .map(formContactMethodToApiContactMethod);

  const apiPerson = {
    ...restObject,
    organization: restObject.organization ? restObject.organization : null,
    isDisabled: stringToBoolean(formValues.isDisabled),
    addresses,
    contactMethods,
  } as IEditablePerson;

  return apiPerson;
}

export function apiPersonToFormPerson(person?: IEditablePerson) {
  if (!person) return undefined;

  // exclude api-specific fields from form values
  const { addresses, contactMethods, ...restObject } = person;

  // split address array into sub-types: MAILING, RESIDENTIAL, BILLING
  const formAddresses = addresses?.map(apiAddressToFormAddress) || [];
  const addressDictionary = toDictionary(formAddresses, 'addressTypeId');

  // split contact methods array into phone and email values
  const formContactMethods = contactMethods?.map(apiContactMethodToFormContactMethod) || [];
  const emailContactMethods = formContactMethods.filter(isEmail);
  const phoneContactMethods = formContactMethods.filter(isPhone);

  const formPerson = {
    ...restObject,
    mailingAddress:
      addressDictionary[AddressTypes.Mailing] ?? getDefaultAddress(AddressTypes.Mailing),
    propertyAddress:
      addressDictionary[AddressTypes.Residential] ?? getDefaultAddress(AddressTypes.Residential),
    billingAddress:
      addressDictionary[AddressTypes.Billing] ?? getDefaultAddress(AddressTypes.Billing),
    emailContactMethods:
      emailContactMethods.length > 0 ? emailContactMethods : [getDefaultContactMethod()],
    phoneContactMethods:
      phoneContactMethods.length > 0 ? phoneContactMethods : [getDefaultContactMethod()],
  } as IEditablePersonForm;

  return formPerson;
}

export function formOrganizationToApiOrganization(
  formValues: IEditableOrganizationForm,
): IEditableOrganization {
  // exclude form-specific fields from API payload object
  const {
    mailingAddress,
    propertyAddress,
    billingAddress,
    emailContactMethods,
    phoneContactMethods,
    ...restObject
  } = formValues;

  const addresses = [mailingAddress, propertyAddress, billingAddress]
    .filter(hasAddress)
    .map(formAddressToApiAddress);

  const contactMethods = [...emailContactMethods, ...phoneContactMethods]
    .filter(hasContactMethod)
    .map(formContactMethodToApiContactMethod);

  const apiOrganization = {
    ...restObject,
    isDisabled: stringToBoolean(formValues.isDisabled),
    addresses,
    contactMethods,
    persons: undefined, // do not send the list of persons back to the server. will not be saved
  } as IEditableOrganization;

  return apiOrganization;
}

export function apiOrganizationToFormOrganization(organization?: IEditableOrganization) {
  if (!organization) return undefined;

  // exclude api-specific fields from form values
  const { addresses, contactMethods, persons, ...restObject } = organization;

  // split address array into sub-types: MAILING, RESIDENTIAL, BILLING
  const formAddresses = addresses?.map(apiAddressToFormAddress) || [];
  const addressDictionary = toDictionary(formAddresses, 'addressTypeId');

  // split contact methods array into phone and email values
  const formContactMethods = contactMethods?.map(apiContactMethodToFormContactMethod) || [];
  const emailContactMethods = formContactMethods.filter(isEmail);
  const phoneContactMethods = formContactMethods.filter(isPhone);

  // Format person API values - need full names here
  const formPersonList: Partial<IContactPerson>[] = (persons || []).map(p => {
    return { id: p.id, fullName: formatFullName(p) };
  });

  const formValues = {
    ...restObject,
    persons: formPersonList,
    mailingAddress:
      addressDictionary[AddressTypes.Mailing] ?? getDefaultAddress(AddressTypes.Mailing),
    propertyAddress:
      addressDictionary[AddressTypes.Residential] ?? getDefaultAddress(AddressTypes.Residential),
    billingAddress:
      addressDictionary[AddressTypes.Billing] ?? getDefaultAddress(AddressTypes.Billing),
    emailContactMethods:
      emailContactMethods.length > 0 ? emailContactMethods : [getDefaultContactMethod()],
    phoneContactMethods:
      phoneContactMethods.length > 0 ? phoneContactMethods : [getDefaultContactMethod()],
  } as IEditableOrganizationForm;

  return formValues;
}

export function getApiMailingAddress(
  contact: IEditablePerson | IEditableOrganization,
): IBaseAddress | undefined {
  if (!contact) return undefined;

  const addresses: IBaseAddress[] = contact.addresses || [];
  return addresses.find(addr => addr.addressTypeId?.id === AddressTypes.Mailing);
}

export function getApiPersonOrOrgMailingAddress(
  contact: Api_Person | Api_Organization,
): Api_Address | undefined {
  if (!contact) return undefined;

  return (
    (contact as Api_Person).personAddresses?.find(
      addr => addr?.addressUsageType?.id === AddressTypes.Mailing && addr.address,
    )?.address ??
    (contact as Api_Organization).organizationAddresses?.find(
      addr => addr?.addressUsageType?.id === AddressTypes.Mailing,
    )?.address
  );
}

function hasContactMethod(formContactMethod?: IEditableContactMethodForm): boolean {
  if (!formContactMethod) return false;

  const { value, contactMethodTypeCode } = formContactMethod;
  return value !== '' && contactMethodTypeCode !== '';
}

function hasAddress(formAddress?: IEditablePersonAddressForm): boolean {
  if (!formAddress) return false;

  let { streetAddress1, addressTypeId, countryId, municipality, postal } = formAddress;
  countryId = parseInt(countryId.toString()) || 0;

  return (
    streetAddress1 !== '' &&
    addressTypeId !== '' &&
    municipality !== '' &&
    postal !== '' &&
    countryId > 0
  );
}

export function formAddressToApiAddress(
  formAddress: IEditablePersonAddressForm | IEditableOrganizationAddressForm,
) {
  return {
    ...formAddress,
    countryId: parseInt(formAddress?.countryId.toString()) || 0,
    provinceId: isEmpty(formAddress?.provinceId.toString())
      ? undefined
      : parseInt(formAddress?.provinceId.toString()),
    addressTypeId: toTypeCode(formAddress?.addressTypeId),
  } as IEditablePersonAddress | IEditableOrganizationAddress;
}

export function apiAddressToFormAddress(address?: IBaseAddress) {
  if (!address) return undefined;

  return {
    ...address,
    addressTypeId: fromTypeCode(address?.addressTypeId),
  } as IEditablePersonAddressForm | IEditableOrganizationAddressForm;
}

function formContactMethodToApiContactMethod(formContactMethod: IEditableContactMethodForm) {
  return {
    ...formContactMethod,
    value: stringToUndefined(formContactMethod.value),
    contactMethodTypeCode: toTypeCode(formContactMethod.contactMethodTypeCode),
  } as IEditableContactMethod;
}

function apiContactMethodToFormContactMethod(contactMethod?: IEditableContactMethod) {
  if (!contactMethod) return undefined;

  return {
    ...contactMethod,
    contactMethodTypeCode: fromTypeCode(contactMethod.contactMethodTypeCode),
  } as IEditableContactMethodForm;
}

// utility function to map an array to a object dictionary based on a given key
// used to convert the array of addresses into 3 separate fields: mailingAddress, propertyAddress and billingAddress
function toDictionary(array: any[], key: string) {
  return Object.assign({}, ...array.map(obj => ({ [obj[key]]: obj })));
}

function isEmail(contactMethod?: IEditableContactMethodForm): boolean {
  return !!contactMethod && EmailContactMethods.includes(contactMethod.contactMethodTypeCode);
}

function isPhone(contactMethod?: IEditableContactMethodForm): boolean {
  return !!contactMethod && PhoneContactMethods.includes(contactMethod.contactMethodTypeCode);
}

export const getDefaultContact = (organization?: {
  organizationPersons?: Api_OrganizationPerson[];
}): Api_Person | undefined => {
  if (organization?.organizationPersons?.length === 1) {
    return organization?.organizationPersons[0]?.person;
  }
  return undefined;
};

export const getPrimaryContact = (
  primaryContactId: number,
  organization?: {
    organizationPersons?: Api_OrganizationPerson[];
  },
): Api_Person | undefined => {
  return organization?.organizationPersons?.find(op => op.personId === primaryContactId)?.person;
};
