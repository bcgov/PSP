/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

export const PropertyImprovementFormYupSchema = yup.object().shape({
  propertyImprovementTypeCode: yup.string().nullable().required('Improvement type is required'),
  description: yup.string(),
});
