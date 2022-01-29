import { emailContactMethods, phoneContactMethods } from 'constants/contactMethodType';
import { AddressTypes } from 'constants/index';
import {
  ICreateContactAddress,
  ICreateContactAddressForm,
  ICreateContactMethod,
  ICreateContactMethodForm,
  ICreateOrganization,
  ICreateOrganizationForm,
  ICreatePerson,
  ICreatePersonForm,
} from 'interfaces/ICreateContact';
import ITypeCode from 'interfaces/ITypeCode';
import isPlainObject from 'lodash/isPlainObject';
import { stringToNull, stringToTypeCode, typeCodeToString } from 'utils/formUtils';

export function formPersonToApiPerson(formValues: ICreatePersonForm): ICreatePerson {
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
    organizationId: isPlainObject(formValues.organizationId)
      ? typeCodeToString<number>(formValues.organizationId as ITypeCode<number>)
      : undefined,
    addresses,
    contactMethods,
  } as ICreatePerson;

  return apiPerson;
}

export function apiPersonToFormPerson(person?: ICreatePerson) {
  if (!person) return undefined;

  // exclude api-specific fields from form values
  const { addresses, contactMethods, ...restObject } = person;

  // split address array into sub-types: MAILING, RESIDENTIAL, BILLING
  const formAddresses = addresses?.map(apiAddressToFormAddress) || [];
  const addressDictionary = toDictionary(formAddresses, 'addressTypeId');

  // split contact methods array into phone and email values
  const formContactMethods = contactMethods?.map(apiContactMethodToFormContactMethod) || [];

  const formPerson = {
    ...restObject,
    organizationId: stringToTypeCode(person.organizationId),
    mailingAddress: addressDictionary[AddressTypes.Mailing],
    propertyAddress: addressDictionary[AddressTypes.Residential],
    billingAddress: addressDictionary[AddressTypes.Billing],
    emailContactMethods: formContactMethods.filter(isEmail),
    phoneContactMethods: formContactMethods.filter(isPhone),
  } as ICreatePersonForm;

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

function hasContactMethod(formContactMethod: ICreateContactMethodForm): boolean {
  const { value, contactMethodTypeCode } = formContactMethod;
  return value !== '' && contactMethodTypeCode !== '';
}

function hasAddress(formAddress: ICreateContactAddressForm): boolean {
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

function formAddressToApiAddress(formAddress: ICreateContactAddressForm): ICreateContactAddress {
  return {
    ...formAddress,
    countryId: parseInt(formAddress.countryId.toString()) || 0,
    provinceId: parseInt(formAddress.provinceId.toString()) || 0,
    addressTypeId: stringToTypeCode(formAddress.addressTypeId),
  } as ICreateContactAddress;
}

function apiAddressToFormAddress(address?: ICreateContactAddress) {
  if (!address) return undefined;

  return {
    ...address,
    addressTypeId: typeCodeToString(address.addressTypeId),
  } as ICreateContactAddressForm;
}

function formContactMethodToApiContactMethod(formContactMethod: ICreateContactMethodForm) {
  return {
    ...formContactMethod,
    value: stringToNull(formContactMethod.value),
    contactMethodTypeCode: stringToTypeCode(formContactMethod.contactMethodTypeCode),
  } as ICreateContactMethod;
}

function apiContactMethodToFormContactMethod(contactMethod?: ICreateContactMethod) {
  if (!contactMethod) return undefined;

  return {
    ...contactMethod,
    contactMethodTypeCode: typeCodeToString(contactMethod.contactMethodTypeCode),
  } as ICreateContactMethodForm;
}

// utility function to map an array to a object dictionary based on a given key
// used to convert the array of addresses into 3 separate fields: mailingAddress, propertyAddress and billingAddress
function toDictionary(array: any[], key: string) {
  return Object.assign({}, ...array.map(obj => ({ [obj[key]]: obj })));
}

function isEmail(contactMethod?: ICreateContactMethodForm): boolean {
  return !!contactMethod && emailContactMethods.includes(contactMethod.contactMethodTypeCode);
}

function isPhone(contactMethod?: ICreateContactMethodForm): boolean {
  return !!contactMethod && phoneContactMethods.includes(contactMethod.contactMethodTypeCode);
}
