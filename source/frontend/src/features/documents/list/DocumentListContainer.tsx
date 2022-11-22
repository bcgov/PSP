import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import useIsMounted from 'hooks/useIsMounted';
import { Api_DocumentRelationship } from 'models/api/Document';
import { useCallback, useEffect, useState } from 'react';
import { toast } from 'react-toastify';

import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import DocumentListView from './DocumentListView';

interface IDocumentListContainerProps {
  parentId: number;
  relationshipType: DocumentRelationshipType;
  disableAdd?: boolean;
  addButtonText?: string;
}

const DocumentListContainer: React.FunctionComponent<IDocumentListContainerProps> = props => {
  const isMounted = useIsMounted();

  const [documentResults, setDocumentResults] = useState<Api_DocumentRelationship[]>([]);

  const {
    retrieveDocumentRelationship,
    retrieveDocumentRelationshipLoading,
    deleteDocumentRelationship,
  } = useDocumentRelationshipProvider();

  const retrieveDocuments = useCallback(async () => {
    const documents = await retrieveDocumentRelationship(props.relationshipType, props.parentId);
    if (documents !== undefined && isMounted()) {
      setDocumentResults(documents);
    }
  }, [isMounted, retrieveDocumentRelationship, props.relationshipType, props.parentId]);

  useEffect(() => {
    retrieveDocuments();
  }, [retrieveDocuments]);

  const onDelete = async (
    documentRelationship: Api_DocumentRelationship,
  ): Promise<boolean | undefined> => {
    if (documentRelationship.relationshipType !== undefined) {
      let result = await deleteDocumentRelationship(
        documentRelationship.relationshipType,
        documentRelationship,
      );
      if (result && isMounted()) {
        retrieveDocuments();
      }

      return result;
    } else {
      toast.error(
        'Invalid document relationship error during delete. Check responses and try again.',
      );
    }
  };

  return (
    <DocumentListView
      parentId={props.parentId}
      relationshipType={props.relationshipType}
      addButtonText={props.addButtonText}
      isLoading={retrieveDocumentRelationshipLoading}
      documentResults={documentResults}
      onDelete={onDelete}
      refreshDocumentList={retrieveDocuments}
      disableAdd={props.disableAdd}
    />
  );
};

export default DocumentListContainer;
