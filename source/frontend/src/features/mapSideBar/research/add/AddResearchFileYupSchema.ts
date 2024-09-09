import * as Yup from 'yup';

export const AddResearchFileYupSchema = Yup.object().shape({
  name: Yup.string()
    .required('Research File name is required')
    .max(250, 'Research File name must be less than 250 characters'),
  properties: Yup.array().of(
    Yup.object().shape({
      name: Yup.string().max(500, 'Property name must be less than 500 characters'),
      isRetired: Yup.boolean().notOneOf(
        [true],
        'Selected property is retired and can not be added to the file',
      ),
    }),
  ),
});
