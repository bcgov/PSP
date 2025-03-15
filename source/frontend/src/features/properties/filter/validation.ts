/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const PropertyFilterValidationSchema = Yup.object().shape({
  pid: Yup.string().nullable(),
  pin: Yup.string().nullable(),
  // allow numbers & empty string
  coordinates: Yup.object().when('searchBy', {
    is: 'coordinates',
    then: Yup.object().shape({
      latitude: Yup.object().shape({
        degrees: Yup.number()
          .transform((value, original) => (original === '' || isNaN(original) ? undefined : value))
          .integer('Degrees (0 to 89) must be an integer number')
          .min(0, 'Degrees must be greater than ${min}')
          .max(89, 'Degrees must be less than ${max}')
          .required('Numeric value is required'),
        minutes: Yup.number()
          .transform((value, original) => (original === '' || isNaN(original) ? undefined : value))
          .integer('Minutes (0 to 59) must be an integer number')
          .min(0, 'Minutes must be greater than ${min}')
          .max(59, 'Minutes must be less than ${max}')
          .required('Numeric value is required'),
        seconds: Yup.number()
          .transform((value, original) => (original === '' || isNaN(original) ? undefined : value))
          .min(0, 'Seconds must be greater than ${min}')
          .max(59, 'Seconds must be less than ${max}')
          .required('Numeric value is required'),
      }),
      longitude: Yup.object().shape({
        degrees: Yup.number()
          .transform((value, original) => (original === '' || isNaN(original) ? undefined : value))
          .integer('Degrees (0 to 179) must be an integer number')
          .min(0, 'Degrees must be greater than ${min}')
          .max(179, 'Degrees must be less than ${max}')
          .required('Numeric value is required'),
        minutes: Yup.number()
          .transform((value, original) => (original === '' || isNaN(original) ? undefined : value))
          .integer('Minutes (0 to 59) must be an integer number')
          .min(0, 'Minutes must be greater than ${min}')
          .max(59, 'Minutes must be less than ${max}')
          .required('Numeric value is required'),
        seconds: Yup.number()
          .transform((value, original) => (original === '' || isNaN(original) ? undefined : value))
          .min(0, 'Seconds must be greater than ${min}')
          .max(59, 'Seconds must be less than ${max}')
          .required('Numeric value is required'),
      }),
    }),
    otherwise: Yup.object().nullable(),
  }),
});
