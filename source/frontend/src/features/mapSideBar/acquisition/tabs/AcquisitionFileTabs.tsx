import React, { useContext } from 'react';
import { useHistory, useLocation, useParams } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import { NoteTypes } from '@/constants/noteTypes';
import { FileTabs, FileTabType, TabFileView } from '@/features/mapSideBar/shared/detail/FileTabs';
import NoteListView from '@/features/notes/list/NoteListView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_AcquisitionFile, EnumAcquisitionFileType } from '@/models/api/AcquisitionFile';

import { SideBarContext } from '../../context/sidebarContext';
import DocumentsTab from '../../shared/tabs/DocumentsTab';
import AgreementContainer from './agreement/detail/AgreementContainer';
import AgreementView from './agreement/detail/AgreementView';
import { AcquisitionChecklistView } from './checklist/detail/AcquisitionChecklistView';
import CompensationListContainer from './compensation/list/CompensationListContainer';
import CompensationListView from './compensation/list/CompensationListView';
import ExpropriationTabContainer from './expropriation/ExpropriationTabContainer';
import ExpropriationTabContainerView from './expropriation/ExpropriationTabContainerView';
import AcquisitionSummaryView from './fileDetails/detail/AcquisitionSummaryView';
import StakeHolderContainer from './stakeholders/detail/StakeHolderContainer';
import StakeHolderView from './stakeholders/detail/StakeHolderView';

export interface IAcquisitionFileTabsProps {
  acquisitionFile?: Api_AcquisitionFile;
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

  const { setStaleLastUpdatedBy } = useContext(SideBarContext);

  const location = useLocation();
  const history = useHistory();
  const { tab } = useParams<{ tab?: string }>();
  const activeTab = Object.values(FileTabType).find(value => value === tab) ?? defaultTab;

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
    name: 'File details',
  });

  tabViews.push({
    content: (
      <AcquisitionChecklistView
        acquisitionFile={acquisitionFile}
        onEdit={() => setIsEditing(true)}
      />
    ),
    key: FileTabType.CHECKLIST,
    name: 'Checklist',
  });

  if (acquisitionFile?.id && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: (
        <DocumentsTab
          fileId={acquisitionFile.id}
          relationshipType={DocumentRelationshipType.ACQUISITION_FILES}
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

  if (acquisitionFile?.id && hasClaim(Claims.AGREEMENT_VIEW)) {
    tabViews.push({
      content: (
        <AgreementContainer
          acquisitionFileId={acquisitionFile.id}
          View={AgreementView}
          onEdit={() => setIsEditing(true)}
        />
      ),
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

  if (acquisitionFile?.id && hasClaim(Claims.COMPENSATION_REQUISITION_VIEW)) {
    tabViews.push({
      content: (
        <CompensationListContainer
          fileId={acquisitionFile.id}
          file={acquisitionFile}
          View={CompensationListView}
        />
      ),
      key: FileTabType.COMPENSATIONS,
      name: 'Compensation',
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
        />
      ),
      key: FileTabType.EXPROPRIATION,
      name: 'Expropriation',
    });
  }

  const onSetActiveTab = (tab: FileTabType) => {
    let previousTab = activeTab;
    if (previousTab === FileTabType.COMPENSATIONS) {
      const backUrl = location.pathname.split('/compensation-requisition')[0];
      history.push(backUrl);
    }
    setActiveTab(tab);
  };

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
