import React, { useContext, useEffect } from 'react';
import { useParams } from 'react-router-dom';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { NoteTypes } from '@/constants';
import { Claims } from '@/constants/claims';
import { FileTabs, FileTabType, TabFileView } from '@/features/mapSideBar/shared/detail/FileTabs';
import NoteListContainer from '@/features/notes/list/NoteListContainer';
import NoteListView from '@/features/notes/list/NoteListView';
import { PropertyNoteSummaryContainer } from '@/features/notes/list/PropertyNoteSummaryContainer';
import { PropertyNoteSummaryView } from '@/features/notes/list/PropertyNoteSummaryView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { isValidId } from '@/utils';

import { SideBarContext } from '../../context/sidebarContext';
import usePathGenerator from '../../shared/sidebarPathGenerator';
import ManagementDocumentsTab from '../../shared/tabs/ManagementDocumentsTab';
import ActivitiesTab from './activities/ActivitiesTab';
import ManagementSummaryView from './fileDetails/detail/ManagementSummaryView';

export interface IManagementFileTabsProps {
  managementFile?: ApiGen_Concepts_ManagementFile;
  defaultTab: FileTabType;
  setIsEditing: (value: boolean) => void;
}

export const ManagementFileTabs: React.FC<IManagementFileTabsProps> = ({
  managementFile,
  defaultTab,
  setIsEditing,
}) => {
  const { setFullWidthSideBar } = useMapStateMachine();

  const tabViews: TabFileView[] = [];
  const { hasClaim } = useKeycloakWrapper();
  const { setStaleLastUpdatedBy } = useContext(SideBarContext);
  const { tab: paramsTab } = useParams<{ tab?: string }>();
  const activeTab = Object.values(FileTabType).find(value => value === paramsTab) ?? defaultTab;
  const pathGenerator = usePathGenerator();

  const setActiveTab = (tab: FileTabType) => {
    if (activeTab !== tab) {
      pathGenerator.showDetails('management', managementFile.id, tab, false);
    }
  };

  const onChildSuccess = () => {
    setStaleLastUpdatedBy(true);
  };

  tabViews.push({
    content: (
      <ManagementSummaryView managementFile={managementFile} onEdit={() => setIsEditing(true)} />
    ),
    key: FileTabType.FILE_DETAILS,
    name: 'File Details',
  });

  if (isValidId(managementFile?.id)) {
    tabViews.push({
      content: <ActivitiesTab managementFile={managementFile} />,
      key: FileTabType.ACTIVITIES,
      name: 'Activities',
    });
  }

  if (isValidId(managementFile?.id) && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: <ManagementDocumentsTab fileId={managementFile.id} onSuccess={onChildSuccess} />,
      key: FileTabType.DOCUMENTS,
      name: 'Documents',
    });
  }

  if (isValidId(managementFile?.id) && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: (
        <>
          <NoteListContainer
            type={NoteTypes.Management_File}
            entityId={managementFile?.id}
            onSuccess={onChildSuccess}
            NoteListView={NoteListView}
          />
          <PropertyNoteSummaryContainer
            fileProperties={managementFile?.fileProperties ?? []}
            onSuccess={onChildSuccess}
            View={PropertyNoteSummaryView}
          />
        </>
      ),
      key: FileTabType.NOTES,
      name: 'Notes',
    });
  }

  const onSetActiveTab = (tab: FileTabType) => {
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

export default ManagementFileTabs;
