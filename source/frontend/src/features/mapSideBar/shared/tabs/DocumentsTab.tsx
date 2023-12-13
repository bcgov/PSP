import * as React from 'react';

import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';

interface IDocumentsTabProps {
  fileId: number;
  relationshipType: ApiGen_CodeTypes_DocumentRelationType;
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
