import { ICreatePersonForm } from 'interfaces/ICreateContact';
import * as Yup from 'yup';

export function hasPhoneNumber(values: ICreatePersonForm): boolean {
  return values?.phoneContactMethods.some(obj => !!obj.value);
}

export function hasEmail(values: ICreatePersonForm): boolean {
  return values?.emailContactMethods.some(obj => !!obj.value);
}

export function hasAddress(values: ICreatePersonForm): boolean {
  return (
    !!values?.mailingAddress?.streetAddress1 ||
    !!values?.propertyAddress?.streetAddress1 ||
    !!values?.billingAddress?.streetAddress1
  );
}

export const validationSchema = Yup.object().shape({
  firstName: Yup.string().required('First Name is required'),
  surname: Yup.string().required('Last Name is required'),
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
  mailingAddress: Yup.object().shape({
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
});
