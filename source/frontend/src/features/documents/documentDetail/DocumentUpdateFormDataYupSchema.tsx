import * as yup from 'yup';

export const DocumentUpdateFormDataYupSchema = yup.object().shape({
  documentTypeId: yup.string().nullable().required('Document type is required'),
  fileName: yup.string().nullable().max(500).required('File name is required'),
});
