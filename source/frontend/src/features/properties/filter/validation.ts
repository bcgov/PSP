/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

// allow numbers & empty string
const createNumberSchema = (
  label: string,
  min: number,
  max: number,
  isRequired = false,
  isInteger = false,
) => {
  let schema = Yup.number()
    .label(label)
    .transform((value, original) => (original === '' || isNaN(original) ? undefined : value))
    .min(min, '${label} must be greater than ${min}')
    .max(max, '${label} must be less than ${max}');

  if (isRequired) {
    schema = schema.required('Numeric value is required for ${label}');
  }
  if (isInteger) {
    schema = schema.integer(`\${label} (${min} to ${max}) must be an integer number`);
  }

  return schema;
};

export const PropertyFilterValidationSchema = Yup.object().shape({
  pid: Yup.string().nullable(),
  pin: Yup.string().nullable(),
  coordinates: Yup.object().when('searchBy', {
    is: 'coordinates',
    then: Yup.object().shape({
      latitude: Yup.object().shape({
        degrees: createNumberSchema('Degrees', 0, 89, true, true),
        minutes: createNumberSchema('Minutes', 0, 59, true, true),
        seconds: createNumberSchema('Seconds', 0, 59, true, false),
      }),
      longitude: Yup.object().shape({
        degrees: createNumberSchema('Degrees', 0, 179, true, true),
        minutes: createNumberSchema('Minutes', 0, 59, true, true),
        seconds: createNumberSchema('Seconds', 0, 59, true, false),
      }),
    }),
    otherwise: Yup.object().nullable(),
  }),
});
