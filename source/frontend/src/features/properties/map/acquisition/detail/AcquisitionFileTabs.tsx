import { Claims } from 'constants/claims';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { FileTypes } from 'constants/fileTypes';
import { NoteTypes } from 'constants/noteTypes';
import DocumentListContainer from 'features/documents/list/DocumentListContainer';
import { FileTabNames, FileTabs, TabFileView } from 'features/mapSideBar/tabs/FileTabs';
import NoteListView from 'features/notes/list/NoteListView';
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

export const AcquisitionFileTabs: React.FunctionComponent<
  React.PropsWithChildren<IAcquisitionFileTabsProps>
> = ({ acquisitionFile, setContainerState }) => {
  const tabViews: TabFileView[] = [];
  const { hasClaim } = useKeycloakWrapper();

  tabViews.push({
    content: (
      <AcquisitionSummaryView
        acquisitionFile={acquisitionFile}
        onEdit={() =>
          setContainerState({
            isEditing: true,
            activeEditForm: EditFormNames.acquisitionSummary,
          })
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

  if (acquisitionFile?.id && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: (
        <DocumentListContainer
          parentId={acquisitionFile?.id}
          relationshipType={DocumentRelationshipType.ACQUISITION_FILES}
          disableAdd
        />
      ),
      key: FileTabNames.documents,
      name: 'Documents',
    });
  }

  if (acquisitionFile?.id && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: <NoteListView type={NoteTypes.Acquisition_File} entityId={acquisitionFile?.id} />,
      key: FileTabNames.notes,
      name: 'Notes',
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
