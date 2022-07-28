import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { Api_DocumentRelationship } from 'models/api/Document';
import { useCallback, useEffect, useState } from 'react';

import { useDocumentRelationshipProvider } from '../hooks/useDocumentRelationshipProvider';
import DocumentListView from './DocumentListView';

interface IDocumentListContainerProps {
  parentId: number;
  relationshipType: DocumentRelationshipType;
}

const DocumentListContainer: React.FunctionComponent<IDocumentListContainerProps> = props => {
  const [documentResults, setDocumentResults] = useState<Api_DocumentRelationship[]>([]);

  const {
    retrieveDocumentRelationship,
    retrieveDocumentRelationshipLoading,
    deleteDocumentRelationship,
  } = useDocumentRelationshipProvider();

  const retrieveDocuments = useCallback(async () => {
    const documents = await retrieveDocumentRelationship(props.relationshipType, props.parentId);
    if (documents !== undefined) {
      setDocumentResults(documents);
    }
  }, [retrieveDocumentRelationship, props.relationshipType, props.parentId]);

  useEffect(() => {
    retrieveDocuments();
  }, [retrieveDocuments]);

  const deleteHandle = (documentRelationship: Api_DocumentRelationship): Promise<boolean> => {
    return new Promise<boolean>((resolve, reject) => {
      deleteDocumentRelationship(props.relationshipType, documentRelationship).then(result => {
        if (result) {
          retrieveDocuments();
        }
        resolve(!!result);
      });
    });
  };

  return (
    <DocumentListView
      parentId={props.parentId}
      relationshipType={props.relationshipType}
      isLoading={retrieveDocumentRelationshipLoading}
      documentResults={documentResults}
      deleteHandle={deleteHandle}
    />
  );
};

export default DocumentListContainer;
