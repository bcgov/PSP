import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import useIsMounted from 'hooks/useIsMounted';
import { Api_DocumentType, Api_DocumentUploadRequest } from 'models/api/Document';
import { Api_Storage_DocumentTypeMetadataType } from 'models/api/DocumentStorage';
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
  const {
    retrieveDocumentMetadataLoading,
    retrieveDocumentTypeMetadata,
    retrieveDocumentTypes,
  } = useDocumentProvider();

  const { uploadDocument, uploadDocumentLoading } = useDocumentRelationshipProvider();

  const [documentTypes, setDocumentTypes] = useState<Api_DocumentType[]>([]);
  const [documentType, setDocumentType] = useState<string>('');

  const [documentTypeMetadataTypes, setDocumentTypeMetadataTypes] = useState<
    Api_Storage_DocumentTypeMetadataType[]
  >([]);

  useEffect(() => {
    const fetch = async () => {
      const axiosResponse = await retrieveDocumentTypes();
      if (axiosResponse && isMounted()) {
        setDocumentTypes(axiosResponse);
      }
    };

    fetch();
  }, [retrieveDocumentTypes, isMounted]);

  const onDocumentTypeChange = async (changeEvent: ChangeEvent<HTMLInputElement>) => {
    const documentTypeId = Number(changeEvent.target.value);
    const mayanDocumentTypeId = documentTypes.find(x => x.id === documentTypeId)?.mayanId;
    setDocumentType(documentTypeId?.toString() || '');
    if (mayanDocumentTypeId) {
      const results = await retrieveDocumentTypeMetadata(mayanDocumentTypeId);
      if (results?.status === ExternalResultStatus.Success) {
        setDocumentTypeMetadataTypes(results?.payload?.results);
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
