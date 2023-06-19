import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import { useLeaseDetail } from '@/features/leases/hooks/useLeaseDetail';
import { useContext } from 'react';

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
