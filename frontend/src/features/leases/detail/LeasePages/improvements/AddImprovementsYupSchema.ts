import * as Yup from 'yup';

export const AddImprovementsYupSchema = Yup.object().shape({
  improvements: Yup.array().of(
    Yup.object().shape({
      address: Yup.string().max(2000),
      structureSize: Yup.string().max(2000),
      description: Yup.string().max(2000),
    }),
  ),
});
