import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import DocumentListContainer from 'features/documents/list/DocumentListContainer';
import { useLeaseDetail } from 'features/leases/hooks/useLeaseDetail';

const DocumentsPage: React.FunctionComponent = () => {
  const { lease } = useLeaseDetail();
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
