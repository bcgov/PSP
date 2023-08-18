import React, { useState } from 'react';

import { Claims } from '@/constants/claims';
import { DocumentRelationshipType } from '@/constants/documentRelationshipType';
import { NoteTypes } from '@/constants/noteTypes';
import NoteListView from '@/features/notes/list/NoteListView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_ResearchFile } from '@/models/api/ResearchFile';

import { FileTabs, FileTabType, TabFileView } from '../../shared/detail/FileTabs';
import DocumentsTab from '../../shared/tabs/DocumentsTab';
import { FormKeys } from '../FormKeys';
import ResearchSummaryView from './fileDetails/details/ResearchSummaryView';

export interface IResearchTabsContainerProps {
  researchFile?: Api_ResearchFile;
  // The "edit key" identifies which form is currently being edited: e.g.
  //  - property details info,
  //  - research summary,
  //  - research property info
  //  - 'none' means no form is being edited.
  setEditKey: (editKey: FormKeys) => void;
  setEditMode: (isEditing: boolean) => void;
}

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const ResearchTabsContainer: React.FunctionComponent<
  React.PropsWithChildren<IResearchTabsContainerProps>
> = ({ researchFile, setEditMode, setEditKey }) => {
  const tabViews: TabFileView[] = [];
  const { hasClaim } = useKeycloakWrapper();

  tabViews.push({
    content: (
      <ResearchSummaryView
        researchFile={researchFile}
        setEditMode={editable => {
          setEditMode(editable);
          setEditKey(FormKeys.researchSummary);
        }}
      />
    ),
    key: FileTabType.FILE_DETAILS,
    name: 'File Details',
  });

  if (researchFile?.id && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: (
        <DocumentsTab
          fileId={researchFile.id}
          relationshipType={DocumentRelationshipType.RESEARCH_FILES}
        />
      ),
      key: FileTabType.DOCUMENTS,
      name: 'Documents',
    });
  }

  if (researchFile?.id && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: <NoteListView type={NoteTypes.Research_File} entityId={researchFile?.id} />,
      key: FileTabType.NOTES,
      name: 'Notes',
    });
  }

  var defaultTab = FileTabType.FILE_DETAILS;

  const [activeTab, setActiveTab] = useState<FileTabType>(defaultTab);

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
