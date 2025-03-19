import React, { useContext, useEffect } from 'react';
import { useHistory, useLocation, useParams } from 'react-router-dom';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { EnumAcquisitionFileType } from '@/constants/acquisitionFileType';
import * as API from '@/constants/API';
import { Claims } from '@/constants/claims';
import { NoteTypes } from '@/constants/noteTypes';
import { FileTabs, FileTabType, TabFileView } from '@/features/mapSideBar/shared/detail/FileTabs';
import NoteListView from '@/features/notes/list/NoteListView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { exists } from '@/utils';

import CompensationListContainer from '../../compensation/list/CompensationListContainer';
import CompensationListView from '../../compensation/list/CompensationListView';
import { SideBarContext } from '../../context/sidebarContext';
import { ChecklistView } from '../../shared/tabs/checklist/detail/ChecklistView';
import DocumentsTab from '../../shared/tabs/DocumentsTab';
import AgreementContainer from './agreement/detail/AgreementContainer';
import AgreementView from './agreement/detail/AgreementView';
import ExpropriationTabContainer from './expropriation/ExpropriationTabContainer';
import ExpropriationTabContainerView from './expropriation/ExpropriationTabContainerView';
import AcquisitionFileStatusUpdateSolver from './fileDetails/detail/AcquisitionFileStatusUpdateSolver';
import AcquisitionSummaryView from './fileDetails/detail/AcquisitionSummaryView';
import StakeHolderContainer from './stakeholders/detail/StakeHolderContainer';
import StakeHolderView from './stakeholders/detail/StakeHolderView';
import SubFileListContainer from './subFiles/SubFileListContainer';
import SubFileListView from './subFiles/SubFileListView';

export interface IAcquisitionFileTabsProps {
  acquisitionFile?: ApiGen_Concepts_AcquisitionFile;
  defaultTab: FileTabType;
  setIsEditing: (value: boolean) => void;
}

export const AcquisitionFileTabs: React.FC<IAcquisitionFileTabsProps> = ({
  acquisitionFile,
  defaultTab,
  setIsEditing,
}) => {
  const tabViews: TabFileView[] = [];
  const { hasClaim } = useKeycloakWrapper();
  const { setFullWidthSideBar } = useMapStateMachine();

  const { setStaleLastUpdatedBy } = useContext(SideBarContext);

  const location = useLocation();
  const history = useHistory();
  const { tab } = useParams<{ tab?: string }>();
  const activeTab = Object.values(FileTabType).find(value => value === tab) ?? defaultTab;
  const solverStatus = new AcquisitionFileStatusUpdateSolver(acquisitionFile.fileStatusTypeCode);

  const setActiveTab = (tab: FileTabType) => {
    if (activeTab !== tab) {
      history.push(`${tab}`);
    }
  };

  const onChildSuccess = () => {
    setStaleLastUpdatedBy(true);
  };

  tabViews.push({
    content: (
      <AcquisitionSummaryView acquisitionFile={acquisitionFile} onEdit={() => setIsEditing(true)} />
    ),
    key: FileTabType.FILE_DETAILS,
    name: 'File Details',
  });

  tabViews.push({
    content: (
      <ChecklistView
        apiFile={acquisitionFile}
        isFileFinalStatus={!solverStatus.canEditChecklists()}
        showEditButton={true}
        onEdit={() => setIsEditing(true)}
        sectionTypeName={API.ACQUISITION_CHECKLIST_SECTION_TYPES}
        editClaim={Claims.ACQUISITION_EDIT}
        prefix="acq"
      />
    ),
    key: FileTabType.CHECKLIST,
    name: 'Checklist',
  });

  if (acquisitionFile?.id && hasClaim(Claims.AGREEMENT_VIEW)) {
    tabViews.push({
      content: <AgreementContainer acquisitionFileId={acquisitionFile.id} View={AgreementView} />,
      key: FileTabType.AGREEMENTS,
      name: 'Agreements',
    });
  }

  if (acquisitionFile?.id) {
    tabViews.push({
      content: (
        <StakeHolderContainer
          View={StakeHolderView}
          onEdit={() => setIsEditing(true)}
          acquisitionFile={acquisitionFile}
        />
      ),
      key: FileTabType.STAKEHOLDERS,
      name: 'Stakeholders',
    });
  }

  if (
    acquisitionFile?.id &&
    (acquisitionFile.acquisitionTypeCode?.id === EnumAcquisitionFileType.SECTN3 ||
      acquisitionFile.acquisitionTypeCode?.id === EnumAcquisitionFileType.SECTN6)
  ) {
    tabViews.push({
      content: (
        <ExpropriationTabContainer
          acquisitionFile={acquisitionFile}
          View={ExpropriationTabContainerView}
          statusUpdateSolver={solverStatus}
        />
      ),
      key: FileTabType.EXPROPRIATION,
      name: 'Expropriation',
    });
  }

  if (acquisitionFile?.id && hasClaim(Claims.COMPENSATION_REQUISITION_VIEW)) {
    tabViews.push({
      content: (
        <CompensationListContainer
          fileType={ApiGen_CodeTypes_FileTypes.Acquisition}
          file={acquisitionFile}
          View={CompensationListView}
          statusUpdateSolver={solverStatus}
        />
      ),
      key: FileTabType.COMPENSATIONS,
      name: 'Compensation',
    });
  }

  if (exists(acquisitionFile?.id)) {
    tabViews.push({
      content: (
        <SubFileListContainer
          acquisitionFile={acquisitionFile}
          View={SubFileListView}
          statusUpdateSolver={solverStatus}
        />
      ),
      key: FileTabType.SUB_FILES,
      name: 'Sub-Files',
    });
  }

  if (acquisitionFile?.id && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: (
        <DocumentsTab
          fileId={acquisitionFile.id}
          relationshipType={ApiGen_CodeTypes_DocumentRelationType.AcquisitionFiles}
          onSuccess={onChildSuccess}
        />
      ),
      key: FileTabType.DOCUMENTS,
      name: 'Documents',
    });
  }

  if (acquisitionFile?.id && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: (
        <NoteListView
          type={NoteTypes.Acquisition_File}
          entityId={acquisitionFile?.id}
          onSuccess={onChildSuccess}
        />
      ),
      key: FileTabType.NOTES,
      name: 'Notes',
    });
  }

  const onSetActiveTab = (tab: FileTabType) => {
    const previousTab = activeTab;
    if (previousTab === FileTabType.COMPENSATIONS) {
      const backUrl = location.pathname.split('/compensation-requisition')[0];
      history.push(backUrl);
    }
    setActiveTab(tab);
  };

  useEffect(() => {
    if (activeTab === FileTabType.NOTES || activeTab === FileTabType.DOCUMENTS) {
      setFullWidthSideBar(true);
    } else {
      setFullWidthSideBar(false);
    }
    return () => setFullWidthSideBar(false);
  }, [activeTab, setFullWidthSideBar]);

  return (
    <FileTabs
      tabViews={tabViews}
      defaultTabKey={defaultTab}
      activeTab={activeTab}
      setActiveTab={(tab: FileTabType) => {
        onSetActiveTab(tab);
      }}
    />
  );
};

export default AcquisitionFileTabs;
