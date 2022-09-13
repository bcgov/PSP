import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { useApiDocuments } from 'hooks/pims-api/useApiDocuments';
import { Api_Document, Api_DocumentUpdateRequest } from 'models/api/Document';
import { Api_Storage_DocumentTypeMetadataType } from 'models/api/DocumentStorage';
import { ExternalResultStatus } from 'models/api/ExternalResult';
import { useEffect, useState } from 'react';

import { ComposedDocument } from '../ComposedDocument';
import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import { DocumentDetailForm } from './DocumentDetailForm';
import { DocumentDetailView } from './DocumentDetailView';

export interface IDocumentDetailContainerProps {
  relationshipType: DocumentRelationshipType;
  pimsDocument: Api_Document;
  onUpdateSuccess: () => void;
}

export const DocumentDetailContainer: React.FunctionComponent<IDocumentDetailContainerProps> = props => {
  const [document, setDocument] = useState<ComposedDocument>({ pimsDocument: props.pimsDocument });
  const [documentTypeMetadataTypes, setDocumentTypeMetadataTypes] = useState<
    Api_Storage_DocumentTypeMetadataType[]
  >([]);
  const { updateDocument, updateDocumentLoading } = useDocumentRelationshipProvider();
  const [isEditable, setIsEditable] = useState<boolean>(false);

  const { retrieveDocumentMetadata, retrieveDocumentMetadataLoading } = useDocumentProvider();
  const { getDocumentTypeMetadata } = useApiDocuments();

  useEffect(() => {
    const fetch = async () => {
      if (props.pimsDocument.mayanDocumentId !== undefined) {
        const axiosResponse = await retrieveDocumentMetadata(props.pimsDocument.mayanDocumentId);
        if (axiosResponse?.status === ExternalResultStatus.Success) {
          let mayanMetadataResult = axiosResponse.payload.results;

          let mayanFileId: number | undefined = undefined;
          if (mayanMetadataResult !== undefined && mayanMetadataResult?.length > 0) {
            const document = mayanMetadataResult[0]?.document;
            mayanFileId = document.file_latest.id;
          }

          setDocument(document => ({
            ...document,
            mayanMetadata: mayanMetadataResult,
            mayanFileId: mayanFileId,
          }));
        }
      }
    };

    fetch();
  }, [props.pimsDocument.mayanDocumentId, retrieveDocumentMetadata]);

  useEffect(() => {
    const fetch = async () => {
      if (props.pimsDocument.mayanDocumentId !== undefined) {
        const mayanDocumentTypeId = props.pimsDocument.documentType?.mayanId;
        if (mayanDocumentTypeId) {
          const axiosResponse = await getDocumentTypeMetadata(mayanDocumentTypeId);
          if (axiosResponse?.data.status === ExternalResultStatus.Success) {
            let results = axiosResponse?.data.payload.results;
            setDocumentTypeMetadataTypes(results);
          }
        }
      }
    };
    fetch();
  }, [
    props.pimsDocument.mayanDocumentId,
    props.pimsDocument.documentType?.mayanId,
    getDocumentTypeMetadata,
  ]);

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

  const onCancelUpdate = () => {
    setIsEditable(false);
  };

  if (!!isEditable) {
    return (
      <DocumentDetailView
        document={document}
        isLoading={retrieveDocumentMetadataLoading || updateDocumentLoading}
        setIsEditable={setIsEditable}
      />
    );
  } else {
    return (
      <DocumentDetailForm
        document={document}
        isLoading={retrieveDocumentMetadataLoading || updateDocumentLoading}
        onUpdate={onUpdateDocument}
        onCancel={onCancelUpdate}
        mayanMetadataTypes={documentTypeMetadataTypes}
      />
    );
  }
};
