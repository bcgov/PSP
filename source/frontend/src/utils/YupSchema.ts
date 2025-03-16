/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const AccessRequestSchema = Yup.object().shape({
  showOrganization: Yup.boolean(),
  roleId: Yup.number().min(0, 'Invalid Role').required('Required'),
  regionCodeId: Yup.number().min(0, 'Invalid Region').required('Required'),
  note: Yup.string().max(4000, 'Note must be less than 4000 characters'),
  position: Yup.string().max(100, 'Position must be less than 100 characters'),
});

export const UserUpdateSchema = Yup.object().shape({
  email: Yup.string().email().max(200, 'Email must be less than 200 characters'),
  firstName: Yup.string()
    .required('First Name is required')
    .max(50, 'First Name must be less than 50 characters'),
  surname: Yup.string()
    .required('Last Name is required')
    .max(50, 'Last Name must be less than 50 characters'),
  note: Yup.string().max(1000, 'Note must be less than 1000 characters'),
  position: Yup.string().max(100, 'Position must be less than 100 characters'),
  userTypeCode: Yup.object().shape({
    id: Yup.string().required('User type is required'),
  }),
  middleName: Yup.string().max(400, 'Middle Name must be less than 400 characters'),
  regions: Yup.array().min(1, 'A user must have at least one region'),
  roles: Yup.array().min(1, 'A user must have at least one role'),
});

// allow numbers & empty string
export const createNumberSchema = (
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
