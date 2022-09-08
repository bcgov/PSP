import { Claims } from 'constants/claims';
import { FileTypes } from 'constants/fileTypes';
import {
  ResearchTabNames,
  ResearchTabs,
  TabResearchView,
} from 'features/mapSideBar/tabs/ResearchTabs';
import ActivityListView from 'features/research/activities/list/ActivityListView';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import React, { useState } from 'react';

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
export const ResearchTabsContainer: React.FunctionComponent<IResearchTabsContainerProps> = ({
  researchFile,
  setEditMode,
  setEditKey,
}) => {
  const tabViews: TabResearchView[] = [];
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
    key: ResearchTabNames.researchDetails,
    name: 'Research Details',
  });

  if (researchFile?.id && hasClaim(Claims.ACTIVITY_VIEW)) {
    tabViews.push({
      content: (
        <ActivityListView fileId={researchFile.id} fileType={FileTypes.Research}></ActivityListView>
      ),
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
