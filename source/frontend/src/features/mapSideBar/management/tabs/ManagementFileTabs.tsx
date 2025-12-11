import React, { useContext, useEffect, useMemo } from 'react';
import { useParams } from 'react-router-dom';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Claims } from '@/constants/claims';
import { FileTabs, FileTabType, TabFileView } from '@/features/mapSideBar/shared/detail/FileTabs';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { isValidId } from '@/utils';

import { SideBarContext } from '../../context/sidebarContext';
import usePathGenerator from '../../shared/sidebarPathGenerator';
import ManagementDocumentsTab from '../../shared/tabs/ManagementDocumentsTab';
import ActivitiesTab from './activities/ActivitiesTab';
import ManagementSummaryContainer from './fileDetails/detail/ManagementSummaryContainer';
import ManagementSummaryView from './fileDetails/detail/ManagementSummaryView';
import ManagementFileNotesTab from './notes/ManagementFileNotesTab';

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
  const params = useParams();
  const pathGenerator = usePathGenerator();

  const activeTab = useMemo(
    () => Object.values(FileTabType).find(value => value === params['detailType']) ?? defaultTab,
    [defaultTab, params],
  );

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
      <ManagementSummaryContainer
        managementFile={managementFile}
        onFileEdit={() => setIsEditing(true)}
        View={ManagementSummaryView}
      />
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
      content: (
        <ManagementDocumentsTab managementFile={managementFile} onSuccess={onChildSuccess} />
      ),
      key: FileTabType.DOCUMENTS,
      name: 'Documents',
    });
  }

  if (isValidId(managementFile?.id) && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: (
        <ManagementFileNotesTab managementFile={managementFile} onSuccess={onChildSuccess} />
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
