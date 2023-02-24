import { Claims } from 'constants/claims';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import { NoteTypes } from 'constants/noteTypes';
import DocumentListContainer from 'features/documents/list/DocumentListContainer';
import { LeaseFormModel } from 'features/leases/models';
import {
  LeaseFileTabNames,
  LeaseFileTabs,
  LeaseTabFileView,
} from 'features/mapSideBar/tabs/LeaseFileTabs';
import NoteListView from 'features/notes/list/NoteListView';
import { FormikProps } from 'formik';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { IFormLease, ILease } from 'interfaces';
import React from 'react';

import { LeaseContainerState, LeasePageNames, leasePages } from '../LeaseContainer';
import { LeaseTab } from './LeaseTab';

export interface ILeaseTabsContainerProps {
  lease?: ILease;
  refreshLease: () => void;
  setLease: (lease: ILease) => void;
  setContainerState: (value: Partial<LeaseContainerState>) => void;
  isEditing: boolean;
  onEdit?: () => {};
  activeTab?: LeaseFileTabNames;
  formikRef: React.RefObject<FormikProps<LeaseFormModel | IFormLease>>;
}

export const LeaseTabsContainer: React.FC<ILeaseTabsContainerProps> = ({
  lease,
  setContainerState,
  isEditing,
  activeTab,
  formikRef,
}) => {
  const tabViews: LeaseTabFileView[] = [];
  const { hasClaim } = useKeycloakWrapper();

  tabViews.push({
    content: (
      <LeaseTab
        leasePage={leasePages.get(LeasePageNames.DETAILS)}
        onEdit={() =>
          setContainerState({ activeEditForm: LeasePageNames.DETAILS, isEditing: true })
        }
        isEditing={isEditing}
        formikRef={formikRef}
      />
    ),
    key: LeaseFileTabNames.fileDetails,
    name: 'File details',
  });

  tabViews.push({
    content: (
      <LeaseTab
        leasePage={leasePages.get(LeasePageNames.TENANT)}
        onEdit={() => setContainerState({ activeEditForm: LeasePageNames.TENANT, isEditing: true })}
        isEditing={isEditing}
        formikRef={formikRef}
      />
    ),
    key: LeaseFileTabNames.tenant,
    name: 'Tenant',
  });

  tabViews.push({
    content: (
      <LeaseTab
        leasePage={leasePages.get(LeasePageNames.IMPROVEMENTS)}
        onEdit={() =>
          setContainerState({ activeEditForm: LeasePageNames.IMPROVEMENTS, isEditing: true })
        }
        isEditing={isEditing}
        formikRef={formikRef}
      />
    ),
    key: LeaseFileTabNames.improvements,
    name: 'Improvements',
  });

  tabViews.push({
    content: (
      <LeaseTab
        leasePage={leasePages.get(LeasePageNames.INSURANCE)}
        onEdit={() =>
          setContainerState({ activeEditForm: LeasePageNames.INSURANCE, isEditing: true })
        }
        isEditing={isEditing}
        formikRef={formikRef}
      />
    ),
    key: LeaseFileTabNames.insurance,
    name: 'Insurance',
  });

  tabViews.push({
    content: (
      <LeaseTab
        leasePage={leasePages.get(LeasePageNames.DEPOSIT)}
        isEditing={isEditing}
        formikRef={formikRef}
      />
    ),
    key: LeaseFileTabNames.deposit,
    name: 'Deposit',
  });

  tabViews.push({
    content: (
      <LeaseTab
        leasePage={leasePages.get(LeasePageNames.PAYMENTS)}
        isEditing={isEditing}
        formikRef={formikRef}
      />
    ),
    key: LeaseFileTabNames.payments,
    name: 'Payments',
  });

  tabViews.push({
    content: (
      <LeaseTab
        leasePage={leasePages.get(LeasePageNames.SURPLUS)}
        isEditing={isEditing}
        formikRef={formikRef}
      />
    ),
    key: LeaseFileTabNames.surplusDeclaration,
    name: 'Surplus Declaration',
  });

  if (lease?.id && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: (
        <DocumentListContainer
          parentId={lease?.id}
          relationshipType={DocumentRelationshipType.LEASES}
        />
      ),
      key: LeaseFileTabNames.documents,
      name: 'Documents',
    });
  }

  if (lease?.id && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: <NoteListView type={NoteTypes.Lease_File} entityId={lease?.id} />,
      key: LeaseFileTabNames.notes,
      name: 'Notes',
    });
  }

  var defaultTab = LeaseFileTabNames.fileDetails;

  return (
    <LeaseFileTabs
      tabViews={tabViews}
      defaultTabKey={defaultTab}
      activeTab={activeTab ?? defaultTab}
      setActiveTab={(tab: LeaseFileTabNames) => setContainerState({ activeTab: tab })}
    />
  );
};

export default LeaseTabsContainer;
