import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { useApiDocuments } from 'hooks/pims-api/useApiDocuments';
import useIsMounted from 'hooks/useIsMounted';
import { Api_DocumentType, Api_DocumentUploadRequest } from 'models/api/Document';
import { Api_Storage_DocumentTypeMetadataType } from 'models/api/DocumentStorage';
import { ExternalResultStatus } from 'models/api/ExternalResult';
import { ChangeEvent, useEffect, useState } from 'react';

import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import DocumentUploadForm from './DocumentUploadForm';

export interface IDocumentUploadContainerProps {
  parentId: number;
  relationshipType: DocumentRelationshipType;
  onUploadSuccess: () => void;
  onCancel: () => void;
}

export const DocumentUploadContainer: React.FunctionComponent<IDocumentUploadContainerProps> = props => {
  const isMounted = useIsMounted();
  const { getDocumentTypes, getDocumentTypeMetadata } = useApiDocuments();
  const { retrieveDocumentMetadataLoading } = useDocumentProvider();

  const { uploadDocument, uploadDocumentLoading } = useDocumentRelationshipProvider();

  const [documentTypes, setDocumentTypes] = useState<Api_DocumentType[]>([]);
  const [documentType, setDocumentType] = useState<string>('');

  const [documentTypeMetadataTypes, setDocumentTypeMetadataTypes] = useState<
    Api_Storage_DocumentTypeMetadataType[]
  >([]);

  useEffect(() => {
    const fetch = async () => {
      const axiosResponse = await getDocumentTypes();
      if (axiosResponse && isMounted()) {
        setDocumentTypes(axiosResponse.data);
      }
    };

    fetch();
  }, [getDocumentTypes, isMounted]);

  const onDocumentTypeChange = async (changeEvent: ChangeEvent<HTMLInputElement>) => {
    const documentTypeId = Number(changeEvent.target.value);
    const mayanDocumentTypeId = documentTypes.find(x => x.id === documentTypeId)?.mayanId;
    setDocumentType(documentTypeId?.toString() || '');
    if (mayanDocumentTypeId) {
      const axiosResponse = await getDocumentTypeMetadata(mayanDocumentTypeId);
      if (axiosResponse?.data.status === ExternalResultStatus.Success) {
        let results = axiosResponse?.data.payload.results;
        setDocumentTypeMetadataTypes(results);
      }
    }
  };

  const onUploadDocument = async (uploadRequest: Api_DocumentUploadRequest) => {
    await uploadDocument(props.relationshipType, props.parentId, uploadRequest);
    props.onUploadSuccess();
  };

  return (
    <DocumentUploadForm
      isLoading={retrieveDocumentMetadataLoading || uploadDocumentLoading}
      initialDocumentType={documentType}
      documentTypes={documentTypes}
      mayanMetadataTypes={documentTypeMetadataTypes}
      onDocumentTypeChange={onDocumentTypeChange}
      onUploadDocument={onUploadDocument}
      onCancel={props.onCancel}
    />
  );
};
