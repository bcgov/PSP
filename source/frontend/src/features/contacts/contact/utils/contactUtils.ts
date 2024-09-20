import { validateYupSchema, yupToFormErrors } from 'formik';
import isEmpty from 'lodash/isEmpty';

import { CountryCodes } from '@/constants/index';
import { AddressField } from '@/features/contacts/interfaces';
import { ApiGen_Concepts_OrganizationAddress } from '@/models/api/generated/ApiGen_Concepts_OrganizationAddress';
import { ApiGen_Concepts_PersonAddress } from '@/models/api/generated/ApiGen_Concepts_PersonAddress';

import { IEditableOrganizationForm, IEditablePersonForm } from '../../formModels';
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

export const toAddressFields = (
  addresses: ApiGen_Concepts_PersonAddress[] | ApiGen_Concepts_OrganizationAddress[],
) => {
  return addresses
    .sort(sortAddresses)
    .reduce(
      (
        accumulator: AddressField[],
        value: ApiGen_Concepts_PersonAddress | ApiGen_Concepts_OrganizationAddress,
      ) => {
        accumulator.push({
          label: value?.addressUsageType?.description || '',
          streetAddress1: value?.address?.streetAddress1,
          streetAddress2: value?.address?.streetAddress2,
          streetAddress3: value?.address?.streetAddress3,
          municipalityAndProvince: [
            value?.address?.municipality !== undefined ? value?.address?.municipality : '',
            value?.address?.province?.code ?? '',
          ]
            .filter(x => !isEmpty(x))
            .join(' '),
          country:
            value?.address?.country?.code === CountryCodes.Other
              ? value?.address?.countryOther ?? ''
              : value?.address?.country?.description,
          postal: value?.address?.postal,
        });
        return accumulator;
      },
      [],
    );
};
