import * as Yup from 'yup';

export const PropertyContactEditFormYupSchema = Yup.object().shape({
  contact: Yup.object().required('Contact is required'),
  purposeDescription: Yup.string()
    .max(500, 'Purpose description must be at most 500 characters')
    .required('Purpose description is required'),
});
