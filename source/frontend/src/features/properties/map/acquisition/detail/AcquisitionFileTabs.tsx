import { Claims } from 'constants/claims';
import { FileTypes } from 'constants/fileTypes';
import { NoteTypes } from 'constants/noteTypes';
import { FileTabs, FileTabType, TabFileView } from 'features/mapSideBar/tabs/FileTabs';
import NoteListView from 'features/notes/list/NoteListView';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React, { useState } from 'react';
import { useHistory } from 'react-router-dom';

import { ActivityListView } from '../../activity/list/ActivityListView';
import AgreementContainer from '../../agreement/detail/AgreementContainer';
import AgreementView from '../../agreement/detail/AgreementView';
import CompensationListContainer from '../../compensation/list/CompensationListContainer';
import CompensationListView from '../../compensation/list/CompensationListView';
import { FormListView } from '../../form/list/FormListView';
import FormListViewContainer from '../../form/list/FormListViewContainer';
import { AcquisitionContainerState } from '../AcquisitionContainer';
import { EditFormType } from '../EditFormNames';
import { AcquisitionChecklistView } from './checklist/AcquisitionChecklistView';
import AcquisitionDocumentsTab from './documents/AcquisitionDocumentsTab';
import AcquisitionSummaryView from './fileDetails/AcquisitionSummaryView';

export interface IAcquisitionFileTabsProps {
  acquisitionFile?: Api_AcquisitionFile;
  defaultTab: FileTabType;
  setContainerState: (value: Partial<AcquisitionContainerState>) => void;
}

export const AcquisitionFileTabs: React.FC<IAcquisitionFileTabsProps> = ({
  acquisitionFile,
  defaultTab,
  setContainerState,
}) => {
  const tabViews: TabFileView[] = [];
  const { hasClaim } = useKeycloakWrapper();
  const [activeTab, setActiveTab] = useState<FileTabType>(defaultTab);

  const history = useHistory();

  tabViews.push({
    content: (
      <AcquisitionSummaryView
        acquisitionFile={acquisitionFile}
        onEdit={() =>
          setContainerState({
            isEditing: true,
            activeEditForm: EditFormType.ACQUISITION_SUMMARY,
            defaultFileTab: FileTabType.FILE_DETAILS,
          })
        }
      />
    ),
    key: FileTabType.FILE_DETAILS,
    name: 'File details',
  });

  tabViews.push({
    content: (
      <AcquisitionChecklistView
        acquisitionFile={acquisitionFile}
        onEdit={() =>
          setContainerState({
            isEditing: true,
            activeEditForm: EditFormType.ACQUISITION_CHECKLIST,
            defaultFileTab: FileTabType.CHECKLIST,
          })
        }
      />
    ),
    key: FileTabType.CHECKLIST,
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
      key: FileTabType.ACTIVITIES,
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
      key: FileTabType.FORMS,
      name: 'Forms',
    });
  }

  if (acquisitionFile?.id && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: <AcquisitionDocumentsTab acquisitionFileId={acquisitionFile.id} />,
      key: FileTabType.DOCUMENTS,
      name: 'Documents',
    });
  }

  if (acquisitionFile?.id && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: <NoteListView type={NoteTypes.Acquisition_File} entityId={acquisitionFile?.id} />,
      key: FileTabType.NOTES,
      name: 'Notes',
    });
  }

  if (acquisitionFile?.id && hasClaim(Claims.AGREEMENT_VIEW)) {
    tabViews.push({
      content: (
        <AgreementContainer
          acquisitionFileId={acquisitionFile.id}
          View={AgreementView}
          onEdit={() =>
            setContainerState({
              isEditing: true,
              activeEditForm: EditFormType.AGREEMENTS,
              defaultFileTab: FileTabType.AGREEMENTS,
            })
          }
        ></AgreementContainer>
      ),
      key: FileTabType.AGREEMENTS,
      name: 'Agreements',
    });
  }

  if (acquisitionFile?.id && hasClaim(Claims.COMPENSATION_REQUISITION_VIEW)) {
    tabViews.push({
      content: (
        <CompensationListContainer
          fileId={acquisitionFile.id}
          View={CompensationListView}
        ></CompensationListContainer>
      ),
      key: FileTabType.COMPENSATIONS,
      name: 'Compensation',
    });
  }

  const onSetActiveTab = (tab: FileTabType) => {
    let previousTab = activeTab;
    setActiveTab(tab);
    setContainerState({ defaultFileTab: tab });

    if (previousTab === FileTabType.COMPENSATIONS) {
      const backUrl = history.location.pathname.split('compensation-requisition')[0];
      history.push(backUrl);
    }
  };

  return (
    <FileTabs
      tabViews={tabViews}
      defaultTabKey={defaultTab}
      activeTab={activeTab}
      setActiveTab={(tab: FileTabType) => {
        onSetActiveTab(tab);
      }}
    />
  );
};

export default AcquisitionFileTabs;
