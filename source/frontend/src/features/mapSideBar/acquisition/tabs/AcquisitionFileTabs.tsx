import React, { useState } from 'react';
import { useHistory } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import { FileTypes } from '@/constants/fileTypes';
import { NoteTypes } from '@/constants/noteTypes';
import NoteListView from '@/features/notes/list/NoteListView';
import ActivityListView from '@/features/properties/map/activity/list/ActivityListView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_AcquisitionFile, EnumAcquisitionFileType } from '@/models/api/AcquisitionFile';

import { FileTabs, FileTabType, TabFileView } from '../../shared/detail/FileTabs';
import { AcquisitionContainerState } from '../AcquisitionContainer';
import { EditFormType } from '../EditFormNames';
import AgreementContainer from './agreement/detail/AgreementContainer';
import AgreementView from './agreement/detail/AgreementView';
import { AcquisitionChecklistView } from './checklist/detail/AcquisitionChecklistView';
import CompensationListContainer from './compensation/list/CompensationListContainer';
import CompensationListView from './compensation/list/CompensationListView';
import AcquisitionDocumentsTab from './documents/AcquisitionDocumentsTab';
import ExpropriationTabContainer from './expropriation/ExpropriationTabContainer';
import ExpropriationTabcontainerView from './expropriation/ExpropriationTabContainerView';
import AcquisitionSummaryView from './fileDetails/detail/AcquisitionSummaryView';
import StakeHolderContainer from './stakeholders/detail/StakeHolderContainer';
import StakeHolderView from './stakeholders/detail/StakeHolderView';

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

  if (acquisitionFile?.id) {
    tabViews.push({
      content: (
        <StakeHolderContainer
          View={StakeHolderView}
          onEdit={() =>
            setContainerState({
              isEditing: true,
              activeEditForm: EditFormType.STAKEHOLDERS,
              defaultFileTab: FileTabType.STAKEHOLDERS,
            })
          }
          acquisitionFile={acquisitionFile}
        ></StakeHolderContainer>
      ),
      key: FileTabType.STAKEHOLDERS,
      name: 'Stakeholders',
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

  if (
    acquisitionFile?.id &&
    (acquisitionFile.acquisitionTypeCode?.id === EnumAcquisitionFileType.SECTN3 ||
      acquisitionFile.acquisitionTypeCode?.id === EnumAcquisitionFileType.SECTN6)
  ) {
    tabViews.push({
      content: (
        <ExpropriationTabContainer
          acquisitionFileId={acquisitionFile.id}
          acquisitionFileTypeCode={acquisitionFile.acquisitionTypeCode?.id}
          View={ExpropriationTabcontainerView}
        ></ExpropriationTabContainer>
      ),
      key: FileTabType.EXPROPRIATION,
      name: 'Expropriation',
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
