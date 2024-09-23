import { FormikProps } from 'formik';
import React, { useCallback, useContext, useEffect, useReducer, useRef, useState } from 'react';
import * as Yup from 'yup';

import LeaseIcon from '@/assets/images/lease-icon.svg?react';
import GenericModal from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Claims } from '@/constants';
import { useLeaseDetail } from '@/features/leases';
import { AddLeaseYupSchema } from '@/features/leases/add/AddLeaseYupSchema';
import LeaseChecklistContainer from '@/features/leases/detail/LeasePages/checklist/LeaseChecklistContainer';
import DepositsContainer from '@/features/leases/detail/LeasePages/deposits/DepositsContainer';
import DetailContainer from '@/features/leases/detail/LeasePages/details/DetailContainer';
import DocumentsPage from '@/features/leases/detail/LeasePages/documents/DocumentsPage';
import { ImprovementsContainer } from '@/features/leases/detail/LeasePages/improvements/ImprovementsContainer';
import InsuranceContainer from '@/features/leases/detail/LeasePages/insurance/InsuranceContainer';
import PeriodPaymentsContainer from '@/features/leases/detail/LeasePages/payment/PeriodPaymentsContainer';
import { PeriodPaymentsYupSchema } from '@/features/leases/detail/LeasePages/payment/PeriodPaymentsYupSchema';
import PeriodPaymentsView, {
  IPeriodPaymentsViewProps,
} from '@/features/leases/detail/LeasePages/payment/table/periods/PaymentPeriodsView';
import LeaseStakeholderContainer from '@/features/leases/detail/LeasePages/stakeholders/LeaseStakeholderContainer';
import Surplus from '@/features/leases/detail/LeasePages/surplus/Surplus';
import { LeaseFormModel } from '@/features/leases/models';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';

import { SideBarContext } from '../context/sidebarContext';
import MapSideBarLayout from '../layout/MapSideBarLayout';
import SidebarFooter from '../shared/SidebarFooter';
import { StyledFormWrapper } from '../shared/styles';
import LeaseHeader from './common/LeaseHeader';
import { LeaseFileTabNames } from './detail/LeaseFileTabs';
import LeaseRouter from './tabs/LeaseRouter';
import ViewSelector from './ViewSelector';

export interface ILeaseContainerProps {
  leaseId: number;
  onClose?: () => void;
}

// Interface for our internal state
export interface LeaseContainerState {
  isEditing: boolean;
  activeEditForm?: LeasePageNames;
  activeTab?: LeaseFileTabNames;
  showConfirmModal: boolean;
}

const initialState: LeaseContainerState = {
  isEditing: false,
  activeEditForm: undefined,
  activeTab: undefined,
  showConfirmModal: false,
};

export interface LeasePageProps<T> {
  isEditing: boolean;
  onEdit?: (isEditing: boolean) => void;
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
  onSuccess: () => void;
  componentView: React.FunctionComponent<React.PropsWithChildren<T>>;
}

export interface ILeasePage<T> {
  pageName: LeasePageNames;
  component: React.FunctionComponent<React.PropsWithChildren<LeasePageProps<T>>>;
  componentView?: React.FunctionComponent<React.PropsWithChildren<T>>;
  title: string;
  description?: string;
  validation?: Yup.ObjectSchema<any>;
  claims?: string[] | string;
  editable?: boolean;
}

export enum LeasePageNames {
  DETAILS = 'details',
  TENANT = 'tenant',
  EDIT_TENANT = 'edit-tenant',
  PAYEE = 'payee',
  EDIT_PAYEE = 'edit-payee',
  PAYMENTS = 'payments',
  IMPROVEMENTS = 'improvements',
  INSURANCE = 'insurance',
  DEPOSIT = 'deposit',
  SURPLUS = 'surplus',
  CHECKLIST = 'checklist',
  DOCUMENTS = 'documents',
  CONSULTATIONS = 'consultations',
}

export const leasePages: Map<LeasePageNames, ILeasePage<any>> = new Map<
  LeasePageNames,
  ILeasePage<any>
>([
  [
    LeasePageNames.DETAILS,
    {
      pageName: LeasePageNames.DETAILS,
      component: DetailContainer,
      title: 'Details',
      validation: AddLeaseYupSchema,
    },
  ],
  [
    LeasePageNames.TENANT,
    {
      pageName: LeasePageNames.TENANT,
      component: LeaseStakeholderContainer,
      title: 'Tenant',
    },
  ],
  [
    LeasePageNames.PAYEE,
    {
      pageName: LeasePageNames.PAYEE,
      component: LeaseStakeholderContainer,
      title: 'Payee',
    },
  ],
  [
    LeasePageNames.PAYMENTS,
    {
      pageName: LeasePageNames.PAYMENTS,
      component: PeriodPaymentsContainer,
      title: 'Payments',
      validation: PeriodPaymentsYupSchema,
      componentView: PeriodPaymentsView,
    } as ILeasePage<IPeriodPaymentsViewProps>,
  ],
  [
    LeasePageNames.IMPROVEMENTS,
    {
      pageName: LeasePageNames.IMPROVEMENTS,
      component: ImprovementsContainer,
      title: 'Improvements',
    },
  ],
  [
    LeasePageNames.INSURANCE,
    {
      pageName: LeasePageNames.INSURANCE,
      component: InsuranceContainer,
      title: 'Insurance',
    },
  ],
  [
    LeasePageNames.DEPOSIT,
    { pageName: LeasePageNames.DEPOSIT, component: DepositsContainer, title: 'Deposit' },
  ],
  [
    LeasePageNames.SURPLUS,
    { pageName: LeasePageNames.SURPLUS, component: Surplus, title: 'Surplus Declaration' },
  ],
  [
    LeasePageNames.CHECKLIST,
    {
      pageName: LeasePageNames.CHECKLIST,
      component: LeaseChecklistContainer,
      title: 'Checklist',
    },
  ],
  [
    LeasePageNames.DOCUMENTS,
    {
      pageName: LeasePageNames.DOCUMENTS,
      component: DocumentsPage,
      title: 'Documents',
      claims: Claims.DOCUMENT_VIEW,
    },
  ],
  [
    LeasePageNames.CONSULTATIONS,
    {
      pageName: LeasePageNames.CONSULTATIONS,
      component: LeaseRouter,
      title: 'Approval/Consultations',
      claims: Claims.LEASE_VIEW,
    },
  ],
]);

export const LeaseContainer: React.FC<ILeaseContainerProps> = ({ leaseId, onClose }) => {
  // keep track of our internal container state
  const [containerState, setContainerState] = useReducer(
    (prevState: LeaseContainerState, newState: Partial<LeaseContainerState>) => ({
      ...prevState,
      ...newState,
    }),
    initialState,
  );

  const formikRef = useRef<FormikProps<LeaseFormModel>>(null);

  const close = useCallback(() => onClose && onClose(), [onClose]);
  const { lease, setLease, refresh, loading } = useLeaseDetail(leaseId);
  const {
    setStaleFile,
    staleFile,
    setStaleLastUpdatedBy,
    setLastUpdatedBy,
    staleLastUpdatedBy,
    lastUpdatedBy,
  } = useContext(SideBarContext);

  const [isValid, setIsValid] = useState<boolean>(true);

  const activeTab = containerState.activeTab;
  const { setFullWidthSideBar } = useMapStateMachine();

  const {
    getLastUpdatedBy: { execute: getLastUpdatedBy, loading: getLastUpdatedByLoading },
  } = useLeaseRepository();

  const onChildSuccess = useCallback(() => {
    setStaleLastUpdatedBy(true);
  }, [setStaleLastUpdatedBy]);

  const handleCancelConfirm = () => {
    if (formikRef !== undefined) {
      formikRef.current?.resetForm();
    }
    setIsValid(true);
    setContainerState({
      showConfirmModal: false,
      isEditing: false,
      activeEditForm: undefined,
    });
  };

  const handleSaveClick = async () => {
    await formikRef?.current?.validateForm();
    if (!formikRef?.current?.isValid) {
      setIsValid(false);
    } else {
      setIsValid(true);
    }

    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      formikRef.current?.submitForm();
    }
  };

  const handleCancelClick = () => {
    if (formikRef !== undefined) {
      if (formikRef.current?.dirty) {
        setContainerState({ showConfirmModal: true });
      } else {
        handleCancelConfirm();
      }
    } else {
      handleCancelConfirm();
    }
  };

  const fetchLastUpdatedBy = useCallback(async () => {
    if (leaseId) {
      const retrieved = await getLastUpdatedBy(leaseId);
      if (retrieved !== undefined) {
        setLastUpdatedBy(retrieved);
      } else {
        setLastUpdatedBy(null);
      }
    }
  }, [leaseId, getLastUpdatedBy, setLastUpdatedBy]);

  useEffect(() => {
    if (
      activeTab === LeaseFileTabNames.deposit ||
      activeTab === LeaseFileTabNames.payments ||
      activeTab === LeaseFileTabNames.notes ||
      activeTab === LeaseFileTabNames.documents
    ) {
      setFullWidthSideBar(true);
    } else {
      setFullWidthSideBar(false);
    }
    return () => setFullWidthSideBar(false);
  }, [activeTab, setFullWidthSideBar]);

  useEffect(() => {
    const refreshLease = async () => {
      await refresh();
    };

    if (staleFile) {
      refreshLease();
      setStaleFile(false);
    }
  }, [staleFile, refresh, setStaleFile]);

  useEffect(() => {
    if (lastUpdatedBy === undefined || leaseId !== lastUpdatedBy?.parentId || staleLastUpdatedBy) {
      fetchLastUpdatedBy();
    }
  }, [fetchLastUpdatedBy, lastUpdatedBy, leaseId, staleLastUpdatedBy]);

  return (
    <MapSideBarLayout
      showCloseButton
      onClose={close}
      title={containerState.isEditing ? 'Update Lease / Licence' : 'Lease / Licence'}
      icon={
        <LeaseIcon
          title="Lease file icon"
          width="2.6rem"
          height="2.6rem"
          fill="currentColor"
          className="mr-2"
        />
      }
      header={<LeaseHeader lease={lease} lastUpdatedBy={lastUpdatedBy} />}
      footer={
        containerState.isEditing && (
          <SidebarFooter
            isOkDisabled={formikRef?.current?.isSubmitting}
            onSave={handleSaveClick}
            onCancel={handleCancelClick}
            displayRequiredFieldError={isValid === false}
          />
        )
      }
    >
      <GenericModal
        variant="info"
        display={containerState.showConfirmModal}
        title={'Confirm Changes'}
        message={
          <>
            <p>If you choose to cancel now, your changes will not be saved.</p>
            <p>Do you want to proceed?</p>
          </>
        }
        handleOk={handleCancelConfirm}
        handleCancel={() => setContainerState({ showConfirmModal: false })}
        okButtonText="Yes"
        cancelButtonText="No"
        show
      />
      <StyledFormWrapper>
        <LoadingBackdrop show={loading || getLastUpdatedByLoading} />
        <ViewSelector
          formikRef={formikRef}
          lease={lease}
          refreshLease={refresh}
          setLease={setLease}
          isEditing={containerState.isEditing}
          activeEditForm={containerState.activeEditForm}
          activeTab={containerState.activeTab}
          setContainerState={setContainerState}
          onSuccess={onChildSuccess}
        />
      </StyledFormWrapper>
    </MapSideBarLayout>
  );
};

export default LeaseContainer;
