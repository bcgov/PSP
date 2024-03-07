import React from 'react';
import { useHistory, useParams } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import { NoteTypes } from '@/constants/noteTypes';
import NoteListView from '@/features/notes/list/NoteListView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';

import { SideBarContext } from '../../context/sidebarContext';
import { FileTabs, FileTabType, TabFileView } from '../../shared/detail/FileTabs';
import DocumentsTab from '../../shared/tabs/DocumentsTab';
import ResearchSummaryView from './fileDetails/details/ResearchSummaryView';

export interface IResearchTabsContainerProps {
  researchFile?: ApiGen_Concepts_ResearchFile;
  setIsEditing: (value: boolean) => void;
}

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const ResearchTabsContainer: React.FunctionComponent<
  React.PropsWithChildren<IResearchTabsContainerProps>
> = ({ researchFile, setIsEditing }) => {
  const tabViews: TabFileView[] = [];
  const { hasClaim } = useKeycloakWrapper();

  const { setStaleLastUpdatedBy } = React.useContext(SideBarContext);

  const history = useHistory();
  const defaultTab = FileTabType.FILE_DETAILS;
  const { tab } = useParams<{ tab?: string }>();
  const activeTab = Object.values(FileTabType).find(value => value === tab) ?? defaultTab;

  const setActiveTab = (tab: FileTabType) => {
    if (activeTab !== tab) {
      history.push(`${tab}`);
    }
  };

  const onChildEntityUpdate = () => {
    setStaleLastUpdatedBy(true);
  };
  tabViews.push({
    content: <ResearchSummaryView researchFile={researchFile} setEditMode={setIsEditing} />,
    key: FileTabType.FILE_DETAILS,
    name: 'File Details',
  });

  if (researchFile?.id && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: (
        <DocumentsTab
          fileId={researchFile.id}
          relationshipType={ApiGen_CodeTypes_DocumentRelationType.ResearchFiles}
          onSuccess={onChildEntityUpdate}
        />
      ),
      key: FileTabType.DOCUMENTS,
      name: 'Documents',
    });
  }

  if (researchFile?.id && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: (
        <NoteListView
          type={NoteTypes.Research_File}
          entityId={researchFile?.id}
          onSuccess={onChildEntityUpdate}
        />
      ),
      key: FileTabType.NOTES,
      name: 'Notes',
    });
  }

  return (
    <FileTabs
      tabViews={tabViews}
      defaultTabKey={defaultTab}
      activeTab={activeTab}
      setActiveTab={setActiveTab}
    />
  );
};

export default ResearchTabsContainer;
