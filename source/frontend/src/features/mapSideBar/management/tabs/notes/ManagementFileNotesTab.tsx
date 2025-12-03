import { NoteTypes } from '@/constants/noteTypes';
import NoteListContainer from '@/features/notes/list/NoteListContainer';
import NoteListView from '@/features/notes/list/NoteListView';
import PropertyNoteSummaryContainer from '@/features/notes/list/PropertyNoteSummaryContainer';
import PropertyNoteSummaryView from '@/features/notes/list/PropertyNoteSummaryView';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';

import ManagementStatusUpdateSolver from '../fileDetails/detail/ManagementStatusUpdateSolver';

interface IManagementFileNotesTabProps {
  managementFile: ApiGen_Concepts_ManagementFile;
  onSuccess?: () => void;
}

const ManagementFileNotesTab: React.FunctionComponent<IManagementFileNotesTabProps> = ({
  managementFile,
  onSuccess,
}) => {
  const statusSolver = new ManagementStatusUpdateSolver(managementFile);

  return (
    <>
      <NoteListContainer
        type={NoteTypes.Management_File}
        entityId={managementFile?.id}
        onSuccess={onSuccess}
        statusSolver={statusSolver}
        View={NoteListView}
      />
      <PropertyNoteSummaryContainer
        fileProperties={managementFile?.fileProperties ?? []}
        onSuccess={onSuccess}
        View={PropertyNoteSummaryView}
      />
    </>
  );
};

export default ManagementFileNotesTab;
