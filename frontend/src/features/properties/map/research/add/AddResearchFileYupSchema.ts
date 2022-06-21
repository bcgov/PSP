import * as Yup from 'yup';

export const AddResearchFileYupSchema = Yup.object().shape({
  name: Yup.string()
    .required('Research File name is required')
    .max(500, 'Research File name must be less than 500 characters'),
  properties: Yup.array().of(
    Yup.object().shape({
      name: Yup.string().max(250, 'Property name must be less than 250 characters'),
    }),
  ),
});
