import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import DocumentManagementListContainer from '@/features/documents/list/DocumentManagementListContainer';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';

interface IManagementDocumentsTabProps {
  fileId: number;
  onSuccess?: () => void;
}

const ManagementDocumentsTab: React.FunctionComponent<IManagementDocumentsTabProps> = ({
  fileId,
  onSuccess,
}) => {
  return (
    <>
      <DocumentListContainer
        parentId={fileId.toString()}
        relationshipType={ApiGen_CodeTypes_DocumentRelationType.ManagementFiles}
        onSuccess={onSuccess}
      />
      <DocumentManagementListContainer
        title={'Related Documents'}
        parentId={fileId.toString()}
        relationshipType={ApiGen_CodeTypes_DocumentRelationType.ManagementFiles}
        onSuccess={onSuccess}
        disableAdd={true}
      />
    </>
  );
};

export default ManagementDocumentsTab;
