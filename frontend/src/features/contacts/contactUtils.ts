import {
  ICreateContactAddress,
  ICreateContactAddressForm,
  ICreateContactMethodForm,
  ICreatePerson,
  ICreatePersonForm,
} from 'interfaces/ICreateContact';
import isPlainObject from 'lodash/isPlainObject';
import { stringToNull, stringToTypeCode } from 'utils/formUtils';

export function personCreateFormToApiPerson(formValues: ICreatePersonForm): ICreatePerson {
  const {
    mailingAddress,
    propertyAddress,
    billingAddress,
    organizationId,
    emailContactMethods,
    phoneContactMethods,
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
    ...formValues,
    organizationId: isPlainObject(organizationId) ? (organizationId as any).id : undefined,
    addresses,
    contactMethods,
  } as ICreatePerson;

  return apiPerson;
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
