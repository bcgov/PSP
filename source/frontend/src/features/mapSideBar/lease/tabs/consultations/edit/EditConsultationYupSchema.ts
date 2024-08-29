/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const UpdateConsultationYupSchema = Yup.object().shape({
  consultationTypeCode: Yup.string().required('Consultation type is required'),
  otherDescription: Yup.string().when('consultationTypeCode', {
    is: (consultationTypeCode: string) => consultationTypeCode && consultationTypeCode === 'OTHER',
    then: Yup.string()
      .required('Other description required')
      .max(2000, 'Other description must be at most ${max} characters'),
    otherwise: Yup.string().nullable(),
  }),
  comment: Yup.string().max(500, 'Comment must be at most ${max} characters'),
});
