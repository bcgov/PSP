import * as yup from 'yup';

export const ManagementFileContactFormYupSchema = yup.object().shape({
  contact: yup.object().required('Contact is required'),
  primaryContactId: yup.number().nullable(),
  purposeDescription: yup
    .string()
    // eslint-disable-next-line no-template-curly-in-string
    .max(500, 'Purpose description must be at most ${max} characters')
    .required('Purpose description is required'),
});
