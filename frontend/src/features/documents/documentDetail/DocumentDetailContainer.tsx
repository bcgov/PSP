import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { Api_Document, Api_DocumentUpdateRequest } from 'models/api/Document';
import { ExternalResultStatus } from 'models/api/ExternalResult';
import { useEffect, useState } from 'react';

import { ComposedDocument } from '../ComposedDocument';
import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import { DocumentDetailView } from './DocumentDetailView';

export interface IDocumentDetailContainerProps {
  relationshipType: DocumentRelationshipType;
  pimsDocument: Api_Document;
  onUpdateSuccess: () => void;
  onCancel?: () => void;
}

export const DocumentDetailContainer: React.FunctionComponent<IDocumentDetailContainerProps> = props => {
  const [document, setDocument] = useState<ComposedDocument>({ pimsDocument: props.pimsDocument });
  const { updateDocument, updateDocumentLoading } = useDocumentRelationshipProvider();

  const { retrieveDocumentMetadata, retrieveDocumentMetadataLoading } = useDocumentProvider();
  useEffect(() => {
    const fetch = async () => {
      if (props.pimsDocument.mayanDocumentId !== undefined) {
        const axiosResponse = await retrieveDocumentMetadata(props.pimsDocument.mayanDocumentId);
        if (axiosResponse?.status === ExternalResultStatus.Success) {
          let results = axiosResponse.payload.results;
          setDocument(document => ({
            ...document,
            mayanMetadata: results,
          }));
        }
      }
    };

    fetch();
  }, [props.pimsDocument.mayanDocumentId, retrieveDocumentMetadata]);

  const onUpdateDocument = async (updateRequest: Api_DocumentUpdateRequest) => {
    if (props.pimsDocument.id) {
      let result = await updateDocument(
        props.relationshipType,
        props.pimsDocument.id,
        updateRequest,
      );
      result && props.onUpdateSuccess();
    }
  };
  return (
    <DocumentDetailView
      document={document}
      isLoading={retrieveDocumentMetadataLoading || updateDocumentLoading}
      onUpdate={onUpdateDocument}
      onCancel={props.onCancel}
    />
  );
};
