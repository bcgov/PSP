import * as Yup from 'yup';

import { Api_Storage_DocumentTypeMetadataType } from '@/models/api/DocumentStorage';

export const getDocumentMetadataYupSchema = (
  mayanMetadata: Api_Storage_DocumentTypeMetadataType[],
) => {
  let metadataSchema: Record<string, Yup.StringSchema> = {};
  for (const data of mayanMetadata) {
    if (data.metadata_type?.id && data.required) {
      metadataSchema[`${data.metadata_type?.id}`] = Yup.string().required(
        `${data.metadata_type?.label} is required`,
      );
    }
  }

  return Yup.object().shape({ documentMetadata: Yup.object().shape(metadataSchema) });
};
