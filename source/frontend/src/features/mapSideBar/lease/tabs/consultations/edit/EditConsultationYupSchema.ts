/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

import { ApiGen_CodeTypes_ConsultationOutcomeTypes } from '@/models/api/generated/ApiGen_CodeTypes_ConsultationOutcomeTypes';

export const UpdateConsultationYupSchema = Yup.object().shape({
  consultationTypeCode: Yup.string().required('Consultation type is required'),
  consultationOutcomeTypeCode: Yup.string().required('Consultation outcome is required'),
  otherDescription: Yup.string().when('consultationTypeCode', {
    is: (consultationTypeCode: string) => consultationTypeCode && consultationTypeCode === 'OTHER',
    then: Yup.string()
      .required('Other description required')
      .max(2000, 'Other description must be at most ${max} characters'),
    otherwise: Yup.string().nullable(),
  }),
  comment: Yup.string().when('consultationOutcomeTypeCode', {
    is: (consultationOutcomeTypeCode: string) =>
      consultationOutcomeTypeCode &&
      (consultationOutcomeTypeCode === ApiGen_CodeTypes_ConsultationOutcomeTypes.APPRDENIED ||
        consultationOutcomeTypeCode === ApiGen_CodeTypes_ConsultationOutcomeTypes.CONSDISCONT),
    then: Yup.string()
      .required('Please add comments')
      .max(500, 'Comment must be at most ${max} characters'),
    otherwise: Yup.string().max(500, 'Comment must be at most ${max} characters'),
  }),
});
