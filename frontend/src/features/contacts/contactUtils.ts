import {
  ICreateContactAddress,
  ICreateContactAddressForm,
  ICreatePerson,
  ICreatePersonForm,
} from 'interfaces/ICreateContact';
import isPlainObject from 'lodash/isPlainObject';

export const personCreateFormToApiPerson = (formValues: ICreatePersonForm): ICreatePerson => {
  let apiValues: ICreatePerson = {
    ...formValues,
    organizationId: isPlainObject(formValues.organizationId)
      ? (formValues.organizationId as any).id
      : undefined,
    addresses: [
      addressFormToApiAddress(formValues.mailingAddress),
      // TODO: FIXME:
      // addressFormToApiAddress(formValues.propertyAddress),
      // addressFormToApiAddress(formValues.billingAddress),
    ],
  };

  return apiValues;
};

const addressFormToApiAddress = (formAddress: ICreateContactAddressForm): ICreateContactAddress => {
  return {
    ...formAddress,
    countryId: parseInt(formAddress.countryId.toString()) || 0,
    provinceId: parseInt(formAddress.provinceId.toString()) || 0,
  };
};
