import * as Yup from 'yup';

export const PropertyContactEditFormYupSchema = Yup.object().shape({
  contact: Yup.object().required('Contact is required'),
});
