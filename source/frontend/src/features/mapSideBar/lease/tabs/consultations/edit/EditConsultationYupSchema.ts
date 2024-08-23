/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const UpdateConsultationYupSchema = Yup.object().shape({
  consultationTypeCode: Yup.string().required('Consultation type is required'),
  otherDescription: Yup.string().when('consultationTypeCode', {
    is: (consultationTypeCode: string) => consultationTypeCode && consultationTypeCode === 'OTHER',
    then: Yup.string()
      .required('Other Description required')
      .max(2000, 'Other Description must be at most ${max} characters'),
    otherwise: Yup.string().nullable(),
  }),
  comment: Yup.string().max(500, 'Other name must be at most ${max} characters'),
});
