import * as Yup from 'yup';

import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';

export const getDocumentMetadataYupSchema = (
  mayanMetadata: ApiGen_Mayan_DocumentTypeMetadataType[],
) => {
  const metadataSchema: Record<string, Yup.StringSchema> = {};
  for (const data of mayanMetadata) {
    if (data.metadata_type?.id && data.required) {
      metadataSchema[`${data.metadata_type?.id}`] = Yup.string().required(
        `${data.metadata_type?.label} is required`,
      );
    }
  }

  return Yup.object().shape({
    documentMetadata: Yup.object().shape(metadataSchema),
    documentTypeId: Yup.string().required('Document type is required'),
  });
};
