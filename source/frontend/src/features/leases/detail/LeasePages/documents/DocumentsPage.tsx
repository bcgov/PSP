import { useContext } from 'react';

import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';

const DocumentsPage: React.FunctionComponent = () => {
  const { lease } = useContext(LeaseStateContext);
  return (
    <>
      <DocumentListContainer
        parentId={lease?.id?.toString() || ''}
        relationshipType={DocumentRelationshipType.LEASES}
      />
    </>
  );
};

export default DocumentsPage;
