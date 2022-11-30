import { Claims } from 'constants/claims';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { FileTypes } from 'constants/fileTypes';
import DocumentListContainer from 'features/documents/list/DocumentListContainer';
import { FileTabNames, FileTabs, TabFileView } from 'features/mapSideBar/tabs/FileTabs';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import React, { useState } from 'react';

import { ActivityListView } from '../activity/list/ActivityListView';
import ResearchSummaryView from './detail/ResearchSummaryView';
import { FormKeys } from './FormKeys';

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
    key: FileTabNames.fileDetails,
    name: 'Research Details',
  });

  if (researchFile?.id && hasClaim(Claims.ACTIVITY_VIEW)) {
    tabViews.push({
      content: (
        <ActivityListView fileId={researchFile.id} fileType={FileTypes.Research}></ActivityListView>
      ),
      key: FileTabNames.activities,
      name: 'Activities',
    });
  }

  if (researchFile?.id && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: (
        <DocumentListContainer
          parentId={researchFile?.id}
          relationshipType={DocumentRelationshipType.RESEARCH_FILES}
          disableAdd
        />
      ),
      key: FileTabNames.documents,
      name: 'Documents',
    });
  }

  var defaultTab = FileTabNames.fileDetails;

  const [activeTab, setActiveTab] = useState<FileTabNames>(defaultTab);

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
