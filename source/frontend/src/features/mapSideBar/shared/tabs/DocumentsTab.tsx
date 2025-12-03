import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import { IUpdateDocumentsStrategy } from '@/features/documents/models/IUpdateDocumentsStrategy';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';

interface IDocumentsTabProps {
  fileId: number;
  relationshipType: ApiGen_CodeTypes_DocumentRelationType;
  title?: string;
  statusSolver?: IUpdateDocumentsStrategy;
  onSuccess?: () => void;
}

const DocumentsTab: React.FunctionComponent<IDocumentsTabProps> = ({
  fileId,
  relationshipType,
  statusSolver,
  title = 'File Documents',
  onSuccess,
}) => {
  return (
    <DocumentListContainer
      title={title}
      parentId={fileId.toString()}
      relationshipType={relationshipType}
      statusSolver={statusSolver}
      onSuccess={onSuccess}
    />
  );
};

export default DocumentsTab;
