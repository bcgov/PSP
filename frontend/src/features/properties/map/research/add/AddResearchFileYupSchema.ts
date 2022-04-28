import * as Yup from 'yup';

export const AddResearchFileYupSchema = Yup.object().shape({
  name: Yup.string().required('Research File name is required'),
  properties: Yup.array(),
});
