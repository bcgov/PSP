import { FormikProps } from 'formik';
import { useHistory, useLocation } from 'react-router-dom';

import { Claims, NoteTypes } from '@/constants';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import { LeaseFormModel } from '@/features/leases/models';
import NoteListView from '@/features/notes/list/NoteListView';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_CodeTypes_LeasePaymentReceivableTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePaymentReceivableTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';

import CompensationListContainer from '../../compensation/list/CompensationListContainer';
import CompensationListView from '../../compensation/list/CompensationListView';
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
  refreshLease,
  isEditing,
  activeTab,
  formikRef,
  onSuccess,
}) => {
  const tabViews: LeaseTabFileView[] = [];
  const { hasClaim } = useKeycloakWrapper();
  const stakeholderPageName =
    lease?.paymentReceivableType.id === 'RCVBL'
      ? LeaseFileTabNames.tenant
      : LeaseFileTabNames.payee;
  const stakeHolderTypeName = lease?.paymentReceivableType.id === 'RCVBL' ? 'Tenant' : 'Payee';

  const location = useLocation();
  const history = useHistory();

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
    name: 'File Details',
  });

  tabViews.push({
    content: (
      <LeaseTab
        leasePage={leasePages.get(LeasePageNames.CONSULTATIONS)}
        isEditing={isEditing}
        formikRef={formikRef}
        onSuccess={onSuccess}
      />
    ),
    key: LeaseFileTabNames.consultations,
    name: 'Approval/Consultations',
  });

  tabViews.push({
    content: (
      <LeaseTab
        leasePage={leasePages.get(LeasePageNames.CHECKLIST)}
        onEdit={() => setContainerState({ isEditing: true })}
        isEditing={isEditing}
        formikRef={formikRef}
        onSuccess={() => {
          setContainerState({ isEditing: false });
          refreshLease();
        }}
      />
    ),
    key: LeaseFileTabNames.checklist,
    name: 'Checklist',
  });

  tabViews.push({
    content: (
      <LeaseTab
        leasePage={
          lease?.paymentReceivableType.id === 'RCVBL'
            ? leasePages.get(LeasePageNames.PAYEE)
            : leasePages.get(LeasePageNames.TENANT)
        }
        onEdit={() => setContainerState({ activeEditForm: LeasePageNames.TENANT, isEditing: true })}
        isEditing={isEditing}
        formikRef={formikRef}
        onSuccess={onSuccess}
      />
    ),
    key: stakeholderPageName,
    name: stakeHolderTypeName,
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

  if (
    lease?.id &&
    (lease.paymentReceivableType.id === ApiGen_CodeTypes_LeasePaymentReceivableTypes.PYBLBCTFA ||
      lease.paymentReceivableType.id === ApiGen_CodeTypes_LeasePaymentReceivableTypes.PYBLMOTI) &&
    hasClaim(Claims.COMPENSATION_REQUISITION_VIEW)
  ) {
    tabViews.push({
      content: (
        <CompensationListContainer
          fileType={ApiGen_CodeTypes_FileTypes.Lease}
          file={lease}
          View={CompensationListView}
        />
      ),
      key: LeaseFileTabNames.compensation,
      name: 'Compensation',
    });
  }

  const defaultTab = LeaseFileTabNames.fileDetails;

  const onSetActiveTab = (tab: LeaseFileTabNames) => {
    const previousTab = activeTab;
    if (previousTab === LeaseFileTabNames.compensation) {
      const backUrl = location.pathname.split('/compensation-requisition')[0];
      history.push(backUrl);
    }
    setContainerState({ activeTab: tab });
  };

  return (
    <LeaseFileTabs
      tabViews={tabViews}
      defaultTabKey={defaultTab}
      activeTab={activeTab ?? defaultTab}
      setActiveTab={(tab: LeaseFileTabNames) => onSetActiveTab(tab)}
    />
  );
};

export default LeaseTabsContainer;
