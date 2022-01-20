import {
  ICreateContactAddress,
  ICreateContactAddressForm,
  ICreateContactMethodForm,
  ICreateOrganization,
  ICreateOrganizationForm,
  ICreatePerson,
  ICreatePersonForm,
} from 'interfaces/ICreateContact';
import isPlainObject from 'lodash/isPlainObject';
import { stringToNull, stringToTypeCode } from 'utils/formUtils';

export function personCreateFormToApiPerson(formValues: ICreatePersonForm): ICreatePerson {
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
    .map(addressFormToApiAddress);

  const contactMethods = [...emailContactMethods, ...phoneContactMethods]
    .filter(hasContactMethod)
    .map(formContactMethod => ({
      ...formContactMethod,
      value: stringToNull(formContactMethod.value),
      contactMethodTypeCode: stringToTypeCode(formContactMethod.contactMethodTypeCode),
    }));

  const apiPerson = {
    ...restObject,
    organizationId: isPlainObject(formValues.organizationId)
      ? (formValues.organizationId as any).id
      : undefined,
    addresses,
    contactMethods,
  } as ICreatePerson;

  return apiPerson;
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
    .map(addressFormToApiAddress);

  const contactMethods = [...emailContactMethods, ...phoneContactMethods]
    .filter(hasContactMethod)
    .map(formContactMethod => ({
      ...formContactMethod,
      value: stringToNull(formContactMethod.value),
      contactMethodTypeCode: stringToTypeCode(formContactMethod.contactMethodTypeCode),
    }));

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

function addressFormToApiAddress(formAddress: ICreateContactAddressForm): ICreateContactAddress {
  return {
    ...formAddress,
    countryId: parseInt(formAddress.countryId.toString()) || 0,
    provinceId: parseInt(formAddress.provinceId.toString()) || 0,
    addressTypeId: stringToTypeCode(formAddress.addressTypeId),
  } as ICreateContactAddress;
}
