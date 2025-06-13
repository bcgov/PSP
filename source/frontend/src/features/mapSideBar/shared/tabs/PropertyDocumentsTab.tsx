import DocumentFileListContainer from '@/features/documents/list/DocumentFileListContainer';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';

interface IPropertyDocumentsTabProps {
  fileId: number;
  onSuccess?: () => void;
}

const PropertyDocumentsTab: React.FunctionComponent<IPropertyDocumentsTabProps> = ({
  fileId,
  onSuccess,
}) => {
  return (
    <>
      <DocumentListContainer
        title={'Property Documents'}
        parentId={fileId.toString()}
        relationshipType={ApiGen_CodeTypes_DocumentRelationType.Properties}
        onSuccess={onSuccess}
      />
      <DocumentFileListContainer
        title={'PIMS Files Documents'}
        parentId={fileId.toString()}
        relationshipType={ApiGen_CodeTypes_DocumentRelationType.Properties}
        onSuccess={onSuccess}
        disableAdd={true}
      />
    </>
  );
};

export default PropertyDocumentsTab;
