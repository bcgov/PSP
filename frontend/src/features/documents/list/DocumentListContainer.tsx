import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { Api_DocumentRelationship } from 'models/api/Document';
import { useEffect, useState } from 'react';

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
  useEffect(() => {
    const fetch = async () => {
      const documents = await retrieveDocumentRelationship(props.relationshipType, props.parentId);
      if (documents !== undefined) {
        setDocumentResults(documents);
      }
    };

    fetch();
  }, [props.parentId, props.relationshipType, retrieveDocumentRelationship]);

  const deleteHandle = (documentRelationship: Api_DocumentRelationship) => {
    deleteDocumentRelationship(props.relationshipType, documentRelationship);
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
