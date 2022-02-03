import { emailContactMethods, phoneContactMethods } from 'constants/contactMethodType';
import { AddressTypes } from 'constants/index';
import {
  getDefaultAddress,
  getDefaultContactMethod,
  ICreateOrganization,
  ICreateOrganizationForm,
  IEditableContactMethod,
  IEditableContactMethodForm,
  IEditablePerson,
  IEditablePersonAddress,
  IEditablePersonAddressForm,
  IEditablePersonForm,
} from 'interfaces/editable-contact';
import { stringToBoolean, stringToNull, stringToTypeCode, typeCodeToString } from 'utils/formUtils';

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

export function organizationCreateFormToApiOrganization(
  formValues: ICreateOrganizationForm,
): ICreateOrganization {
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
    addresses,
    contactMethods,
  } as ICreateOrganization;

  return apiOrganization;
}

function hasContactMethod(formContactMethod?: IEditableContactMethodForm): boolean {
  if (!formContactMethod) return false;

  const { value, contactMethodTypeCode } = formContactMethod;
  return value !== '' && contactMethodTypeCode !== '';
}

function hasAddress(formAddress?: IEditablePersonAddressForm): boolean {
  if (!formAddress) return false;

  let { streetAddress1, addressTypeId, countryId, provinceId, municipality, postal } = formAddress;
  countryId = parseInt(countryId.toString()) || 0;
  provinceId = parseInt(provinceId.toString()) || 0;

  return (
    streetAddress1 !== '' &&
    addressTypeId !== '' &&
    municipality !== '' &&
    postal !== '' &&
    countryId > 0 &&
    provinceId > 0
  );
}

function formAddressToApiAddress(formAddress: IEditablePersonAddressForm): IEditablePersonAddress {
  return {
    ...formAddress,
    countryId: parseInt(formAddress.countryId.toString()) || 0,
    provinceId: parseInt(formAddress.provinceId.toString()) || 0,
    addressTypeId: stringToTypeCode(formAddress.addressTypeId),
  } as IEditablePersonAddress;
}

function apiAddressToFormAddress(address?: IEditablePersonAddress) {
  if (!address) return undefined;

  return {
    ...address,
    addressTypeId: typeCodeToString(address.addressTypeId),
  } as IEditablePersonAddressForm;
}

function formContactMethodToApiContactMethod(formContactMethod: IEditableContactMethodForm) {
  return {
    ...formContactMethod,
    value: stringToNull(formContactMethod.value),
    contactMethodTypeCode: stringToTypeCode(formContactMethod.contactMethodTypeCode),
  } as IEditableContactMethod;
}

function apiContactMethodToFormContactMethod(contactMethod?: IEditableContactMethod) {
  if (!contactMethod) return undefined;

  return {
    ...contactMethod,
    contactMethodTypeCode: typeCodeToString(contactMethod.contactMethodTypeCode),
  } as IEditableContactMethodForm;
}

// utility function to map an array to a object dictionary based on a given key
// used to convert the array of addresses into 3 separate fields: mailingAddress, propertyAddress and billingAddress
function toDictionary(array: any[], key: string) {
  return Object.assign({}, ...array.map(obj => ({ [obj[key]]: obj })));
}

function isEmail(contactMethod?: IEditableContactMethodForm): boolean {
  return !!contactMethod && emailContactMethods.includes(contactMethod.contactMethodTypeCode);
}

function isPhone(contactMethod?: IEditableContactMethodForm): boolean {
  return !!contactMethod && phoneContactMethods.includes(contactMethod.contactMethodTypeCode);
}
