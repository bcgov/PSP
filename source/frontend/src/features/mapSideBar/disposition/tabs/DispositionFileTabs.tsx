import React, { useContext } from 'react';
import { useHistory, useParams } from 'react-router-dom';

import * as API from '@/constants/API';
import { Claims } from '@/constants/claims';
import { NoteTypes } from '@/constants/noteTypes';
import { FileTabs, FileTabType, TabFileView } from '@/features/mapSideBar/shared/detail/FileTabs';
import DocumentsTab from '@/features/mapSideBar/shared/tabs/DocumentsTab';
import NoteListView from '@/features/notes/list/NoteListView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';

import { SideBarContext } from '../../context/sidebarContext';
import { ChecklistView } from '../../shared/tabs/checklist/detail/ChecklistView';
import DispositionSummaryView from './fileDetails/detail/DispositionSummaryView';
import OffersAndSaleContainer from './offersAndSale/OffersAndSaleContainer';
import OffersAndSaleContainerView from './offersAndSale/OffersAndSaleContainerView';

export interface IDispositionFileTabsProps {
  dispositionFile?: ApiGen_Concepts_DispositionFile;
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
      <DispositionSummaryView dispositionFile={dispositionFile} onEdit={() => setIsEditing(true)} />
    ),
    key: FileTabType.FILE_DETAILS,
    name: 'File details',
  });

  if (dispositionFile?.id) {
    tabViews.push({
      content: (
        <OffersAndSaleContainer
          dispositionFile={dispositionFile}
          View={OffersAndSaleContainerView}
        ></OffersAndSaleContainer>
      ),
      key: FileTabType.OFFERS_AND_SALE,
      name: 'Offers & Sale',
    });
  }

  tabViews.push({
    content: (
      <ChecklistView
        onEdit={() => setIsEditing(true)}
        sectionTypeName={API.DISPOSITION_CHECKLIST_SECTION_TYPES}
        editClaim={Claims.DISPOSITION_EDIT}
        prefix="dsp"
        apiFile={dispositionFile}
      />
    ),
    key: FileTabType.CHECKLIST,
    name: 'Checklist',
  });

  if (dispositionFile?.id && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: (
        <DocumentsTab
          fileId={dispositionFile.id}
          relationshipType={ApiGen_CodeTypes_DocumentRelationType.DispositionFiles}
          onSuccess={onChildSuccess}
        />
      ),
      key: FileTabType.DOCUMENTS,
      name: 'Documents',
    });
  }

  if (dispositionFile?.id && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: (
        <NoteListView
          type={NoteTypes.Disposition_File}
          entityId={dispositionFile?.id}
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
