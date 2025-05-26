import React, { useContext, useEffect } from 'react';
import { useHistory, useParams } from 'react-router-dom';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Claims } from '@/constants/claims';
import { NoteTypes } from '@/constants/noteTypes';
import { FileTabs, FileTabType, TabFileView } from '@/features/mapSideBar/shared/detail/FileTabs';
import NoteListView from '@/features/notes/list/NoteListView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { isValidId } from '@/utils';

import { SideBarContext } from '../../context/sidebarContext';
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
        <NoteListView
          type={NoteTypes.Management_File}
          entityId={managementFile?.id}
          onSuccess={onChildSuccess}
        />
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
