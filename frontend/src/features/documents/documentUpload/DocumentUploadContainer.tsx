import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { useApiDocuments } from 'hooks/pims-api/useApiDocuments';
import useIsMounted from 'hooks/useIsMounted';
import { Api_DocumentType, Api_UploadRequest } from 'models/api/Document';
import { useEffect, useState } from 'react';

import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import DocumentUploadView from './DocumentUploadView';

export interface IDocumentUploadContainerProps {
  parentId: number;
  relationshipType: DocumentRelationshipType;
  onUploadSuccess: () => void;
  onCancel: () => void;
}

export const DocumentUploadContainer: React.FunctionComponent<IDocumentUploadContainerProps> = props => {
  const isMounted = useIsMounted();
  const { getDocumentTypes } = useApiDocuments();
  const { retrieveDocumentMetadataLoading } = useDocumentProvider();

  const { uploadDocument, uploadDocumentLoading } = useDocumentRelationshipProvider();

  const [documentTypes, setDocumentTypes] = useState<Api_DocumentType[]>([]);

  useEffect(() => {
    const fetch = async () => {
      const axiosResponse = await getDocumentTypes();
      if (axiosResponse && isMounted()) {
        setDocumentTypes(axiosResponse.data);
      }
    };

    fetch();
  }, [getDocumentTypes, isMounted]);

  const onDocumentTypeChange = (param: any) => {
    // Todo: This will guide the retrieval of the metadata types
  };

  const onUploadDocument = async (uploadRequest: Api_UploadRequest) => {
    await uploadDocument(props.relationshipType, props.parentId, uploadRequest);
    props.onUploadSuccess();
  };

  return (
    <DocumentUploadView
      documentTypes={documentTypes}
      isLoading={retrieveDocumentMetadataLoading || uploadDocumentLoading}
      mayanMetadata={[]}
      onDocumentTypeChange={onDocumentTypeChange}
      onUploadDocument={onUploadDocument}
      onCancel={props.onCancel}
    />
  );
};
