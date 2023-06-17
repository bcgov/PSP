import * as Yup from 'yup';

export const UpdatePropertyYupSchema = Yup.object().shape({
  propertyName: Yup.string().max(500, 'Property name must be less than 500 characters'),
  documentReference: Yup.string().max(2000, 'Document reference must be less than 2000 characters'),
  researchSummary: Yup.string().max(4000, 'Summary notes must be less than 4000 characters'),
});
