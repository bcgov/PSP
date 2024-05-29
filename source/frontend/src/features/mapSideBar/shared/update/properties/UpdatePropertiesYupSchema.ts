import * as Yup from 'yup';

export const UpdatePropertiesYupSchema = Yup.object().shape({
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
