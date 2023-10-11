import * as React from 'react';

import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';

interface IDocumentsTabProps {
  fileId: number;
  relationshipType: DocumentRelationshipType;
  onSuccess?: () => void;
}

const DocumentsTab: React.FunctionComponent<IDocumentsTabProps> = ({
  fileId,
  relationshipType,
  onSuccess,
}) => {
  return (
    <>
      <DocumentListContainer
        title="File Documents"
        parentId={fileId.toString()}
        relationshipType={relationshipType}
        onSuccess={onSuccess}
      />
    </>
  );
};

export default DocumentsTab;
