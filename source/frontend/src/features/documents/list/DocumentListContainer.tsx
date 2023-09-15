import { useCallback, useEffect, useState } from 'react';
import { toast } from 'react-toastify';

import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import useIsMounted from '@/hooks/util/useIsMounted';
import { Api_DocumentRelationship } from '@/models/api/Document';

import { DocumentRow } from '../ComposedDocument';
import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import DocumentListView from './DocumentListView';

interface IDocumentListContainerProps {
  parentId: string;
  relationshipType: DocumentRelationshipType;
  disableAdd?: boolean;
  addButtonText?: string;
  title?: string;
}

const DocumentListContainer: React.FunctionComponent<
  React.PropsWithChildren<IDocumentListContainerProps>
> = props => {
  const isMounted = useIsMounted();

  const [documentResults, setDocumentResults] = useState<DocumentRow[]>([]);

  const {
    retrieveDocumentRelationship,
    retrieveDocumentRelationshipLoading,
    deleteDocumentRelationship,
  } = useDocumentRelationshipProvider();

  const retrieveDocuments = useCallback(async () => {
    const documents = await retrieveDocumentRelationship(props.relationshipType, props.parentId);
    if (documents !== undefined && isMounted()) {
      setDocumentResults([
        ...documents
          .filter((x): x is Api_DocumentRelationship => !!x?.document)
          .map(x => DocumentRow.fromApi(x)),
      ]);
    }
  }, [isMounted, retrieveDocumentRelationship, props.relationshipType, props.parentId]);

  useEffect(() => {
    retrieveDocuments();
  }, [retrieveDocuments]);

  const onDelete = async (
    documentRelationship: Api_DocumentRelationship,
  ): Promise<boolean | undefined> => {
    if (documentRelationship.relationshipType !== null) {
      let result = await deleteDocumentRelationship(
        documentRelationship.relationshipType,
        documentRelationship,
      );
      if (result && isMounted()) {
        updateCallback();
      }

      return result;
    } else {
      toast.error(
        'Invalid document relationship error during delete. Check responses and try again.',
      );
    }
  };

  const onSuccess = async () => {
    updateCallback();
  };

  const updateCallback = useCallback(() => {
    retrieveDocuments();
  }, [retrieveDocuments]);

  return (
    <DocumentListView
      parentId={props.parentId}
      relationshipType={props.relationshipType}
      addButtonText={props.addButtonText}
      isLoading={retrieveDocumentRelationshipLoading}
      documentResults={documentResults}
      onDelete={onDelete}
      onSuccess={onSuccess}
      disableAdd={props.disableAdd}
      title={props.title}
    />
  );
};

export default DocumentListContainer;
