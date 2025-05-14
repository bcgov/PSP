import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';

interface IDocumentsTabProps {
  fileId: number;
  relationshipType: ApiGen_CodeTypes_DocumentRelationType;
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
