import { Claims } from 'constants/claims';
import { FileTypes } from 'constants/fileTypes';
import { NoteTypes } from 'constants/noteTypes';
import { FileTabNames, FileTabs, TabFileView } from 'features/mapSideBar/tabs/FileTabs';
import NoteListView from 'features/notes/list/NoteListView';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React, { useState } from 'react';

import { ActivityListView } from '../../activity/list/ActivityListView';
import { FormListView } from '../../form/list/FormListView';
import FormListViewContainer from '../../form/list/FormListViewContainer';
import { AcquisitionContainerState } from '../AcquisitionContainer';
import { EditFormNames } from '../EditFormNames';
import { AcquisitionChecklistView } from './checklist/AcquisitionChecklistView';
import AcquisitionDocumentsTab from './documents/AcquisitionDocumentsTab';
import AcquisitionSummaryView from './fileDetails/AcquisitionSummaryView';

export interface IAcquisitionFileTabsProps {
  acquisitionFile?: Api_AcquisitionFile;
  defaultTab: FileTabNames;
  setContainerState: (value: Partial<AcquisitionContainerState>) => void;
}

export const AcquisitionFileTabs: React.FC<IAcquisitionFileTabsProps> = ({
  acquisitionFile,
  defaultTab,
  setContainerState,
}) => {
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
            defaultFileTab: FileTabNames.fileDetails,
          })
        }
      />
    ),
    key: FileTabNames.fileDetails,
    name: 'File details',
  });

  tabViews.push({
    content: (
      <AcquisitionChecklistView
        acquisitionFile={acquisitionFile}
        onEdit={() =>
          setContainerState({
            isEditing: true,
            activeEditForm: EditFormNames.acquisitionChecklist,
            defaultFileTab: FileTabNames.checklist,
          })
        }
      />
    ),
    key: FileTabNames.checklist,
    name: 'Checklist',
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

  if (acquisitionFile?.id && hasClaim(Claims.FORM_VIEW)) {
    tabViews.push({
      content: (
        <FormListViewContainer
          View={FormListView}
          fileId={acquisitionFile.id}
          fileType={FileTypes.Acquisition}
        ></FormListViewContainer>
      ),
      key: FileTabNames.forms,
      name: 'Forms',
    });
  }

  if (acquisitionFile?.id && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: <AcquisitionDocumentsTab acquisitionFileId={acquisitionFile.id} />,
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
