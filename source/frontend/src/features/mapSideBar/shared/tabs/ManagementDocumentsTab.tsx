import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import DocumentManagementListContainer from '@/features/documents/list/DocumentManagementListContainer';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';

import ManagementStatusUpdateSolver from '../../management/tabs/fileDetails/detail/ManagementStatusUpdateSolver';

interface IManagementDocumentsTabProps {
  managementFile: ApiGen_Concepts_ManagementFile;
  onSuccess?: () => void;
}

const ManagementDocumentsTab: React.FunctionComponent<IManagementDocumentsTabProps> = ({
  managementFile,
  onSuccess,
}) => {
  const statusSolver = new ManagementStatusUpdateSolver(managementFile);

  return (
    <>
      <DocumentListContainer
        parentId={managementFile.id.toString()}
        relationshipType={ApiGen_CodeTypes_DocumentRelationType.ManagementFiles}
        onSuccess={onSuccess}
        disableAdd={!statusSolver.canEditDocuments()}
      />
      <DocumentManagementListContainer
        title={'Related Documents'}
        parentId={managementFile.id.toString()}
        relationshipType={ApiGen_CodeTypes_DocumentRelationType.ManagementFiles}
        onSuccess={onSuccess}
        disableAdd={true}
      />
    </>
  );
};

export default ManagementDocumentsTab;
