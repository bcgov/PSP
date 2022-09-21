import { Claims } from 'constants/claims';
import { FileTypes } from 'constants/fileTypes';
import { FileTabNames, FileTabs, TabFileView } from 'features/mapSideBar/tabs/FileTabs';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React, { useState } from 'react';

import { ActivityListView } from '../../activity/list/ActivityListView';
import { AcquisitionContainerState } from '../AcquisitionContainer';
import { EditFormNames } from '../EditFormNames';
import AcquisitionSummaryView from './AcquisitionSummaryView';

export interface IAcquisitionFileTabsProps {
  acquisitionFile?: Api_AcquisitionFile;
  setContainerState: (value: Partial<AcquisitionContainerState>) => void;
}

export const AcquisitionFileTabs: React.FunctionComponent<IAcquisitionFileTabsProps> = ({
  acquisitionFile,
  setContainerState,
}) => {
  const tabViews: TabFileView[] = [];
  const { hasClaim } = useKeycloakWrapper();

  tabViews.push({
    content: (
      <AcquisitionSummaryView
        acquisitionFile={acquisitionFile}
        onEdit={() =>
          setContainerState({ isEditing: true, activeEditForm: EditFormNames.acquisitionSummary })
        }
      />
    ),
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

export default AcquisitionFileTabs;
