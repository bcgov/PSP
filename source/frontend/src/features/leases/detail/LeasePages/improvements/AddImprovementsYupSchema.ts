import * as Yup from 'yup';

export const AddImprovementsYupSchema = Yup.object().shape({
  improvements: Yup.array().of(
    Yup.object().shape({
      address: Yup.string().max(2000, 'Address field must be at most 2000 characters'),
      structureSize: Yup.string().max(2000, 'Structure size field must be at most 2000 characters'),
      description: Yup.string().max(2000, 'Description field must be at most 2000 characters'),
    }),
  ),
});
