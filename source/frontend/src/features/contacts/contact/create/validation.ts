import * as Yup from 'yup';

import { IEditableOrganizationForm, IEditablePersonForm } from '@/interfaces/editable-contact';

export function hasPhoneNumber(values: IEditablePersonForm | IEditableOrganizationForm): boolean {
  return values?.phoneContactMethods.some(obj => !!obj.value);
}

export function hasEmail(values: IEditablePersonForm | IEditableOrganizationForm): boolean {
  return values?.emailContactMethods.some(obj => !!obj.value);
}

export function hasAddress(values: IEditablePersonForm | IEditableOrganizationForm): boolean {
  return (
    !!values?.mailingAddress?.streetAddress1 ||
    !!values?.propertyAddress?.streetAddress1 ||
    !!values?.billingAddress?.streetAddress1
  );
}

// validation schema common to Persons and Organizations
const baseSchema = Yup.object().shape({
  emailContactMethods: Yup.array().of(
    Yup.object().shape({
      value: Yup.string().email('Invalid email address'),
      contactMethodTypeCode: Yup.string().when('value', {
        is: (value: string) => !!value,
        then: Yup.string().required('Email type is required'),
      }),
    }),
  ),
  phoneContactMethods: Yup.array().of(
    Yup.object().shape({
      contactMethodTypeCode: Yup.string().when('value', {
        is: (value: string) => !!value,
        then: Yup.string().required('Phone type is required'),
      }),
    }),
  ),
  mailingAddress: Yup.object().when('useOrganizationAddress', {
    is: (useOrganizationAddress: boolean) => {
      return !useOrganizationAddress;
    },
    then: Yup.object().shape({
      municipality: Yup.string().when('streetAddress1', {
        is: (streetAddress1: string) => !!streetAddress1,
        then: Yup.string().required('City is required'),
      }),
      postal: Yup.string().when('streetAddress1', {
        is: (streetAddress1: string) => !!streetAddress1,
        then: Yup.string().required('Postal Code is required'),
      }),
      countryId: Yup.string().when('streetAddress1', {
        is: (streetAddress1: string) => !!streetAddress1,
        then: Yup.string().required('Country is required'),
      }),
      // $otherCountry - you can prefix properties with $ to specify a property that is dependent on context passed in by validate()
      provinceId: Yup.string().when(['streetAddress1', 'countryId', '$otherCountry'], {
        is: (streetAddress1: string, countryId: string, otherCountry: string) =>
          !!streetAddress1 && countryId !== otherCountry,
        then: Yup.string().required('Province/State is required'),
      }),
    }),
  }),
  propertyAddress: Yup.object().shape({
    municipality: Yup.string().when('streetAddress1', {
      is: (streetAddress1: string) => !!streetAddress1,
      then: Yup.string().required('City is required'),
    }),
    postal: Yup.string().when('streetAddress1', {
      is: (streetAddress1: string) => !!streetAddress1,
      then: Yup.string().required('Postal Code is required'),
    }),
    countryId: Yup.string().when('streetAddress1', {
      is: (streetAddress1: string) => !!streetAddress1,
      then: Yup.string().required('Country is required'),
    }),
    // $otherCountry - you can prefix properties with $ to specify a property that is dependent on context passed in by validate()
    provinceId: Yup.string().when(['streetAddress1', 'countryId', '$otherCountry'], {
      is: (streetAddress1: string, countryId: string, otherCountry: string) =>
        !!streetAddress1 && countryId !== otherCountry,
      then: Yup.string().required('Province/State is required'),
    }),
  }),
  billingAddress: Yup.object().shape({
    municipality: Yup.string().when('streetAddress1', {
      is: (streetAddress1: string) => !!streetAddress1,
      then: Yup.string().required('City is required'),
    }),
    postal: Yup.string().when('streetAddress1', {
      is: (streetAddress1: string) => !!streetAddress1,
      then: Yup.string().required('Postal Code is required'),
    }),
    countryId: Yup.string().when('streetAddress1', {
      is: (streetAddress1: string) => !!streetAddress1,
      then: Yup.string().required('Country is required'),
    }),
    // $otherCountry - you can prefix properties with $ to specify a property that is dependent on context passed in by validate()
    provinceId: Yup.string().when(['streetAddress1', 'countryId', '$otherCountry'], {
      is: (streetAddress1: string, countryId: string, otherCountry: string) =>
        !!streetAddress1 && countryId !== otherCountry,
      then: Yup.string().required('Province/State is required'),
    }),
  }),
  comment: Yup.string().max(2000, 'Comments must be at most 2000 characters'),
});

export const PersonValidationSchema = baseSchema.shape({
  firstName: Yup.string().required('First Name is required'),
  surname: Yup.string().required('Last Name is required'),
});

export const OrganizationValidationSchema = baseSchema.shape({
  name: Yup.string().required('Organization Name is required'),
});
