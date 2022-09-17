import { Claims } from 'constants/claims';
import { FileTypes } from 'constants/fileTypes';
import { FileTabNames, FileTabs, TabFileView } from 'features/mapSideBar/tabs/FileTabs';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React, { useState } from 'react';

import { ActivityListView } from '../../activity/list/ActivityListView';
import { AcquisitionContainerState } from '../AcquisitionContainer';

export interface IAcquisitionSummaryTabsProps {
  acquisitionFile?: Api_AcquisitionFile;
  setContainerState: (value: Partial<AcquisitionContainerState>) => void;
}

export const AcquisitionSummaryTabs: React.FunctionComponent<IAcquisitionSummaryTabsProps> = ({
  acquisitionFile,
  setContainerState,
}) => {
  const tabViews: TabFileView[] = [];
  const { hasClaim } = useKeycloakWrapper();

  tabViews.push({
    content: <></>, // TODO: implement PSP-3271 !!
    key: FileTabNames.fileDetails,
    name: 'File details',
  });

  if (acquisitionFile?.id && hasClaim(Claims.ACTIVITY_VIEW)) {
    tabViews.push({
      content: (
        <ActivityListView
          fileId={acquisitionFile.id}
          fileType={FileTypes.Acquisition}
        ></ActivityListView>
      ),
      key: FileTabNames.activities,
      name: 'Activities',
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

export default AcquisitionSummaryTabs;
