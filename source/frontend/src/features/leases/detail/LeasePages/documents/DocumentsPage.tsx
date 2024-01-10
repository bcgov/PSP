import { useContext } from 'react';

import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';

const DocumentsPage: React.FunctionComponent = () => {
  const { lease } = useContext(LeaseStateContext);
  return (
    <>
      <DocumentListContainer
        parentId={lease?.id?.toString() || ''}
        relationshipType={ApiGen_CodeTypes_DocumentRelationType.Leases}
      />
    </>
  );
};

export default DocumentsPage;
