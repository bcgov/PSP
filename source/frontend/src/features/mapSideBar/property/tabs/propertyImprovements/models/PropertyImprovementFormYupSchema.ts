/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

export const PropertyImprovementFormYupSchema = yup.object().shape({
  name: yup
    .string()
    .nullable()
    .required('Improvement name is required')
    .max(500, 'Name field must be at most ${max} characters'),
  improvementTypeCode: yup.string().nullable().required('Improvement type is required'),
  improvementStatusCode: yup.string().nullable().required('Improvement status is required'),
  description: yup.string().max(2000, 'Description field must be at most ${max} characters'),
  improvementDate: yup.string().nullable(),
});
