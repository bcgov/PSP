/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

export const AddDispositionFormYupSchema = yup.object().shape({
  fileName: yup.string().max(200, 'Disposition file name must be at most ${max} characters'),
  referenceNumber: yup
    .string()
    .max(200, 'Disposition reference number must be at most ${max} characters'),
  dispositionStatusTypeCode: yup.string().required(),
  dispositionTypeCode: yup.string().required(),
  dispositionTypeOther: yup.string().when('dispositionTypeCode', {
    is: (dispositionTypeCode: string) => dispositionTypeCode && dispositionTypeCode === 'OTHER',
    then: yup
      .string()
      .required('Other Disposition type is required')
      .max(200, 'Other Disposition type must be at most ${max} characters'),
    otherwise: yup.string().nullable(),
  }),
  initiatingDocumentTypeCode: yup.string(),
  initiatingDocumentTypeOther: yup.string().when('initiatingDocumentTypeCode', {
    is: (initiatingDocumentTypeCode: string) =>
      initiatingDocumentTypeCode && initiatingDocumentTypeCode === 'OTHER',
    then: yup
      .string()
      .required('Other Iniating Document type is required')
      .max(200, 'Other Iniating Document type must be at most ${max} characters'),
    otherwise: yup.string().nullable(),
  }),
  regionCode: yup.string().required('Ministry region is required'),
});
