import * as React from 'react';

import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';

interface IDocumentsTabProps {
  fileId: number;
  relationshipType: DocumentRelationshipType;
  title?: string;
  onSuccess?: () => void;
}

const DocumentsTab: React.FunctionComponent<IDocumentsTabProps> = ({
  fileId,
  relationshipType,
  title = 'File Documents',
  onSuccess,
}) => {
  return (
    <DocumentListContainer
      title={title}
      parentId={fileId.toString()}
      relationshipType={relationshipType}
      onSuccess={onSuccess}
    />
  );
};

export default DocumentsTab;
