import {
  ResearchTabNames,
  ResearchTabs,
  TabResearchView,
} from 'features/mapSideBar/tabs/ResearchTabs';
import ActivityListView from 'features/research/activities/list/ActivityListView';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import React, { useState } from 'react';

import ResearchSummaryView from './detail/ResearchSummaryView';

export interface IResearchTabsContainerProps {
  researchFile: Api_ResearchFile;
  setEditMode: (isEditing: boolean) => void;
}

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const ResearchTabsContainer: React.FunctionComponent<IResearchTabsContainerProps> = ({
  researchFile,
  setEditMode,
}) => {
  const tabViews: TabResearchView[] = [];

  tabViews.push({
    content: <ResearchSummaryView researchFile={researchFile} setEditMode={setEditMode} />,
    key: ResearchTabNames.researchDetails,
    name: 'Research Details',
  });

  if (researchFile?.id) {
    tabViews.push({
      content: <ActivityListView fileId={researchFile.id}></ActivityListView>,
      key: ResearchTabNames.activities,
      name: 'Activities',
    });
  }

  var defaultTab = ResearchTabNames.researchDetails;

  const [activeTab, setActiveTab] = useState<ResearchTabNames>(defaultTab);

  return (
    <ResearchTabs
      tabViews={tabViews}
      defaultTabKey={defaultTab}
      activeTab={activeTab}
      setActiveTab={setActiveTab}
    />
  );
};

export default ResearchTabsContainer;
