import { Api_Storage_DocumentTypeMetadataType } from 'models/api/DocumentStorage';
import * as Yup from 'yup';

export const getDocumentUploadYupSchema = (
  mayanMetadata: Api_Storage_DocumentTypeMetadataType[],
) => {
  let yupSchema: any = {};
  for (const data of mayanMetadata) {
    if (data.metadata_type?.id && data.required) {
      yupSchema[data.metadata_type?.id || ''] = Yup.string().required(
        `${data.metadata_type?.name} is required`,
      );
    }
  }

  return Yup.object().shape(yupSchema);
};
