import * as Yup from 'yup';

export const UpdateResearchFileYupSchema = Yup.object().shape({
  name: Yup.string()
    .required('Research File name is required')
    .max(250, 'Research File name must be less than 250 characters'),
  roadName: Yup.string().max(4000, 'Road name must be less than 4000 characters'),
  roadAlias: Yup.string().max(4000, 'Road alias must be less than 4000 characters'),
  requestDescription: Yup.string().max(
    4000,
    'Description of request must be less than 4000 characters',
  ),
  researchResult: Yup.string().max(4000, 'Result of request must be less than 4000 characters'),
  expropriationNotes: Yup.string().max(
    4000,
    'Expropriation notes must be less than 4000 characters',
  ),
});
