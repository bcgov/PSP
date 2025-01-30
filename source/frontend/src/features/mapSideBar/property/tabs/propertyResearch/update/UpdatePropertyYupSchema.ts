/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const UpdatePropertyYupSchema = Yup.object().shape({
  propertyName: Yup.string().max(500, 'Property name must be less than ${max} characters'),
  documentReference: Yup.string().max(
    2000,
    'Document reference must be less than ${max} characters',
  ),
  researchSummary: Yup.string().max(4000, 'Summary comments must be less than ${max} characters'),
});
