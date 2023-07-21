import { validateYupSchema, yupToFormErrors } from 'formik';
import isEmpty from 'lodash/isEmpty';

import { CountryCodes } from '@/constants/index';
import { AddressField } from '@/features/contacts/interfaces';
import { IEditableOrganizationForm, IEditablePersonForm } from '@/interfaces/editable-contact';
import { IContactAddress } from '@/interfaces/IContact';

import {
  hasAddress,
  hasEmail,
  hasPhoneNumber,
  OrganizationValidationSchema,
  PersonValidationSchema,
} from '../create/validation';
import { sortAddresses } from '../detail/utils';

export const onValidateOrganization = (
  values: IEditableOrganizationForm,
  otherCountryId?: string,
) => {
  const errors = {} as any;
  try {
    // combine yup schema validation with custom rules
    if (!hasEmail(values) && !hasPhoneNumber(values) && !hasAddress(values)) {
      errors.needsContactMethod =
        'Contacts must have a minimum of one method of contact to be saved. (ex: email,phone or address)';
    }
    validateYupSchema(values, OrganizationValidationSchema, true, {
      otherCountry: otherCountryId,
    });
    return errors;
  } catch (err) {
    return { ...errors, ...yupToFormErrors(err) };
  }
};

export const onValidatePerson = (values: IEditablePersonForm, otherCountryId?: string) => {
  const errors = {} as any;
  try {
    // combine yup schema validation with custom rules
    if (!hasEmail(values) && !hasPhoneNumber(values) && !hasAddress(values)) {
      errors.needsContactMethod =
        'Contacts must have a minimum of one method of contact to be saved. (ex: email,phone or address)';
    }
    validateYupSchema(values, PersonValidationSchema, true, { otherCountry: otherCountryId });

    return errors;
  } catch (err) {
    return { ...errors, ...yupToFormErrors(err) };
  }
};

export const toAddressFields = (addresses: IContactAddress[]) => {
  return addresses
    .sort(sortAddresses)
    .reduce((accumulator: AddressField[], value: IContactAddress) => {
      accumulator.push({
        label: value.addressType.description || '',
        streetAddress1: value.streetAddress1,
        streetAddress2: value.streetAddress2,
        streetAddress3: value.streetAddress3,
        municipalityAndProvince: [
          value.municipality !== undefined ? value.municipality : '',
          value.province?.provinceStateCode ?? '',
        ]
          .filter(x => !isEmpty(x))
          .join(' '),
        country:
          value.country?.countryCode === CountryCodes.Other
            ? value.countryOther ?? ''
            : value.country?.description,
        postal: value.postal,
      });
      return accumulator;
    }, []);
};
