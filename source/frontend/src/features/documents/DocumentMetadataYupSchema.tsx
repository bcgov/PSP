import * as Yup from 'yup';

export const getDocumentMetadataYupSchema = (maxDocumentCount: number) => {
  return Yup.object().shape({
    documents: Yup.array()
      .min(1, 'There needs to be at least one document')
      .max(maxDocumentCount, 'Max number of uploads reached')
      .of(
        Yup.object().shape({
          documentTypeId: Yup.string().required('Document type is required'),
        }),
      ),
  });
};
