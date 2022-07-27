import { Api_Document } from 'models/api/Document';
import { ExternalResultStatus } from 'models/api/ExternalResult';
import { useEffect, useState } from 'react';

import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { ComposedDocument } from './ComposedDocument';
import DocumentDetailView from './DocumentDetailView';

export interface IDocumentDetailContainerProps {
  pimsDocument: Api_Document;
}

export const DocumentDetailContainer: React.FunctionComponent<IDocumentDetailContainerProps> = props => {
  const [document, setDocument] = useState<ComposedDocument>({ pimsDocument: props.pimsDocument });

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
  return <DocumentDetailView metadata={document} isLoading={retrieveDocumentMetadataLoading} />;
};
