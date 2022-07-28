import { SelectOption } from 'components/common/form';
import { useApiDocuments } from 'hooks/pims-api/useApiDocuments';
import useIsMounted from 'hooks/useIsMounted';
import { Api_Document, Api_DocumentType } from 'models/api/Document';
import { ExternalResultStatus } from 'models/api/ExternalResult';
import { useEffect, useState } from 'react';

import { useDocumentProvider } from '../hooks/useDocumentProvider';
import { ComposedDocument } from './ComposedDocument';
import DocumentUploadView from './DocumentUploadView';
import DocumentDetailView from './DocumentUploadView';

export interface IDocumentUploadContainerProps {}

export const DocumentUploadContainer: React.FunctionComponent<IDocumentUploadContainerProps> = props => {
  const isMounted = useIsMounted();
  const { getDocumentTypes } = useApiDocuments();
  const { retrieveDocumentMetadata, retrieveDocumentMetadataLoading } = useDocumentProvider();

  const [documentTypes, setDocumentTypes] = useState<Api_DocumentType[]>([]);

  useEffect(() => {
    const fetch = async () => {
      const axiosResponse = await getDocumentTypes();
      if (axiosResponse && isMounted()) {
        setDocumentTypes(axiosResponse.data);
      }
    };

    fetch();
  }, [retrieveDocumentMetadata]);

  return (
    <DocumentUploadView documentTypes={documentTypes} isLoading={retrieveDocumentMetadataLoading} />
  );
};
