import { Claims } from 'constants/claims';
import { FileTypes } from 'constants/fileTypes';
import { FileTabNames, FileTabs, TabFileView } from 'features/mapSideBar/tabs/FileTabs';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import React, { useState } from 'react';

import { ActivityListView } from '../activity/list/ActivityListView';
import ResearchDocumentsTab from './detail/ResearchDocumentsTab';
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
    name: 'File Details',
  });

  if (researchFile?.id && hasClaim(Claims.ACTIVITY_VIEW) && researchFile?.id === 0) {
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
      content: <ResearchDocumentsTab researchFileId={researchFile.id} />,
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
