import * as yup from 'yup';

export const ManagementFileContactFormYupSchema = yup.object().shape({
  contact: yup.object().required('Contact is required'),
  primaryContactId: yup.number().nullable(),
  purposeDescription: yup
    .string()
    .max(500, 'Purpose description must be at most 500 characters')
    .required('Purpose description is required'),
});
