import React from 'react';
import { useHistory, useParams } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import { FileTabs, FileTabType, TabFileView } from '@/features/mapSideBar/shared/detail/FileTabs';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_DispositionFile } from '@/models/api/DispositionFile';

import DispositionSummaryView from './fileDetails/detail/DispositionSummaryView';

export interface IDispositionFileTabsProps {
  dispositionFile?: Api_DispositionFile;
  defaultTab: FileTabType;
  setIsEditing: (value: boolean) => void;
}

export const DispositionFileTabs: React.FC<IDispositionFileTabsProps> = ({
  dispositionFile,
  defaultTab,
  setIsEditing,
}) => {
  const tabViews: TabFileView[] = [];
  const { hasClaim } = useKeycloakWrapper();
  const history = useHistory();
  const { tab } = useParams<{ tab?: string }>();
  const activeTab = Object.values(FileTabType).find(value => value === tab) ?? defaultTab;

  const setActiveTab = (tab: FileTabType) => {
    if (activeTab !== tab) {
      history.push(`${tab}`);
    }
  };

  tabViews.push({
    content: (
      <DispositionSummaryView dispositionFile={dispositionFile} onEdit={() => setIsEditing(true)} />
    ),
    key: FileTabType.FILE_DETAILS,
    name: 'File details',
  });

  tabViews.push({
    content: <></>,
    key: FileTabType.OFFERS_AND_SALE,
    name: 'Offers & Sale',
  });

  tabViews.push({
    content: <></>,
    key: FileTabType.CHECKLIST,
    name: 'Checklist',
  });

  if (dispositionFile?.id && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: <></>,
      key: FileTabType.DOCUMENTS,
      name: 'Documents',
    });
  }

  if (dispositionFile?.id && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: <></>,
      key: FileTabType.NOTES,
      name: 'Notes',
    });
  }

  const onSetActiveTab = (tab: FileTabType) => {
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

export default DispositionFileTabs;
