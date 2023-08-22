import * as React from 'react';

import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';

interface IDocumentsTabProps {
  fileId: number;
  relationshipType: DocumentRelationshipType;
}

const DocumentsTab: React.FunctionComponent<IDocumentsTabProps> = ({
  fileId,
  relationshipType,
}) => {
  return (
    <>
      <DocumentListContainer
        title="File Documents"
        parentId={fileId.toString()}
        relationshipType={relationshipType}
      />
    </>
  );
};

export default DocumentsTab;
