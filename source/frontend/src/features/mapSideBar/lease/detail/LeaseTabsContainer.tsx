import { FormikProps } from 'formik';

import { Claims, NoteTypes } from '@/constants';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import { LeaseFormModel } from '@/features/leases/models';
import NoteListView from '@/features/notes/list/NoteListView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';

import { LeaseContainerState, LeasePageNames, leasePages } from '../LeaseContainer';
import { LeaseFileTabNames, LeaseFileTabs, LeaseTabFileView } from './LeaseFileTabs';
import { LeaseTab } from './LeaseTab';

export interface ILeaseTabsContainerProps {
  lease?: ApiGen_Concepts_Lease;
  refreshLease: () => void;
  setLease: (lease: ApiGen_Concepts_Lease) => void;
  setContainerState: (value: Partial<LeaseContainerState>) => void;
  isEditing: boolean;
  onEdit?: () => object;
  activeTab?: LeaseFileTabNames;
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
  onSuccess: () => void;
}

export const LeaseTabsContainer: React.FC<ILeaseTabsContainerProps> = ({
  lease,
  setContainerState,
  isEditing,
  activeTab,
  formikRef,
  onSuccess,
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
        onSuccess={onSuccess}
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
        onSuccess={onSuccess}
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
        onSuccess={onSuccess}
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
        onSuccess={onSuccess}
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
        onSuccess={onSuccess}
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
        onSuccess={onSuccess}
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
        onSuccess={onSuccess}
      />
    ),
    key: LeaseFileTabNames.surplusDeclaration,
    name: 'Surplus Declaration',
  });

  if (lease?.id && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: (
        <DocumentListContainer
          parentId={lease?.id.toString()}
          relationshipType={ApiGen_CodeTypes_DocumentRelationType.Leases}
          title="File Documents"
          onSuccess={onSuccess}
        />
      ),
      key: LeaseFileTabNames.documents,
      name: 'Documents',
    });
  }

  if (lease?.id && hasClaim(Claims.NOTE_VIEW)) {
    tabViews.push({
      content: (
        <NoteListView type={NoteTypes.Lease_File} entityId={lease?.id} onSuccess={onSuccess} />
      ),
      key: LeaseFileTabNames.notes,
      name: 'Notes',
    });
  }

  const defaultTab = LeaseFileTabNames.fileDetails;

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
