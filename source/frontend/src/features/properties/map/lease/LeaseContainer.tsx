import { ReactComponent as Fence } from 'assets/images/fence.svg';
import GenericModal from 'components/common/GenericModal';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Claims } from 'constants/claims';
import { useLeaseDetail } from 'features/leases';
import { AddLeaseYupSchema } from 'features/leases/add/AddLeaseYupSchema';
import DepositsContainer from 'features/leases/detail/LeasePages/deposits/DepositsContainer';
import DetailContainer from 'features/leases/detail/LeasePages/details/DetailContainer';
import DocumentsPage from 'features/leases/detail/LeasePages/documents/DocumentsPage';
import ImprovementsContainer from 'features/leases/detail/LeasePages/improvements/ImprovementsContainer';
import InsuranceContainer from 'features/leases/detail/LeasePages/insurance/InsuranceContainer';
import TermPaymentsContainer from 'features/leases/detail/LeasePages/payment/TermPaymentsContainer';
import { TermPaymentsYupSchema } from 'features/leases/detail/LeasePages/payment/TermPaymentsYupSchema';
import Surplus from 'features/leases/detail/LeasePages/surplus/Surplus';
import TenantContainer from 'features/leases/detail/LeasePages/tenant/TenantContainer';
import { LeaseFormModel } from 'features/leases/models';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { LeaseFileTabNames } from 'features/mapSideBar/tabs/LeaseFileTabs';
import { FormikProps } from 'formik';
import { IFormLease } from 'interfaces';
import React, { useCallback, useContext, useEffect, useReducer, useRef } from 'react';
import styled from 'styled-components';
import * as Yup from 'yup';

import { SideBarContext } from '../context/sidebarContext';
import SidebarFooter from '../shared/SidebarFooter';
import LeaseHeader from './common/LeaseHeader';
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

export interface LeasePageProps {
  isEditing: boolean;
  onEdit?: (isEditing: boolean) => void;
  formikRef: React.RefObject<FormikProps<LeaseFormModel | IFormLease>>;
}

export interface ILeasePage {
  component: React.FunctionComponent<React.PropsWithChildren<LeasePageProps>>;
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
  PAYMENTS = 'payments',
  IMPROVEMENTS = 'improvements',
  INSURANCE = 'insurance',
  DEPOSIT = 'deposit',
  SURPLUS = 'surplus',
  DOCUMENTS = 'documents',
}

export const leasePages: Map<LeasePageNames, ILeasePage> = new Map<LeasePageNames, ILeasePage>([
  [
    LeasePageNames.DETAILS,
    {
      component: DetailContainer,
      title: 'Details',
      validation: AddLeaseYupSchema,
    },
  ],
  [
    LeasePageNames.TENANT,
    {
      component: TenantContainer,
      title: 'Tenant',
    },
  ],
  [
    LeasePageNames.PAYMENTS,
    {
      component: TermPaymentsContainer,
      title: 'Payments',
      validation: TermPaymentsYupSchema,
    },
  ],
  [
    LeasePageNames.IMPROVEMENTS,
    {
      component: ImprovementsContainer,
      title: 'Improvements',
    },
  ],
  [
    LeasePageNames.INSURANCE,
    {
      component: InsuranceContainer,
      title: 'Insurance',
    },
  ],
  [LeasePageNames.DEPOSIT, { component: DepositsContainer, title: 'Deposit' }],
  [LeasePageNames.SURPLUS, { component: Surplus, title: 'Surplus Declaration' }],
  [
    LeasePageNames.DOCUMENTS,
    { component: DocumentsPage, title: 'Documents', claims: Claims.DOCUMENT_VIEW },
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

  const formikRef = useRef<FormikProps<LeaseFormModel | IFormLease>>(null);

  const close = useCallback(() => onClose && onClose(), [onClose]);
  const { lease, setLease, refresh } = useLeaseDetail(leaseId);
  const { setFullWidth } = useContext(SideBarContext);

  const activeTab = containerState.activeTab;
  useEffect(() => {
    if (activeTab === LeaseFileTabNames.deposit || activeTab === LeaseFileTabNames.payments) {
      setFullWidth(true);
    } else {
      setFullWidth(false);
    }
  }, [activeTab, setFullWidth]);

  if (lease === undefined) {
    return <LoadingBackdrop show={true} parentScreen={true} />;
  }

  const handleCancelConfirm = () => {
    if (formikRef !== undefined) {
      formikRef.current?.resetForm();
    }
    setContainerState({
      showConfirmModal: false,
      isEditing: false,
      activeEditForm: undefined,
    });
  };

  const handleSaveClick = () => {
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

  return (
    <MapSideBarLayout
      showCloseButton
      onClose={close}
      title={containerState.isEditing ? 'Update Lease / License' : 'Lease / License'}
      icon={
        <Fence
          title="Lease file icon"
          width="2.6rem"
          height="2.6rem"
          fill="currentColor"
          className="mr-2"
        />
      }
      header={<LeaseHeader lease={lease} />}
      footer={
        containerState.isEditing && (
          <SidebarFooter
            isOkDisabled={formikRef?.current?.isSubmitting}
            onSave={handleSaveClick}
            onCancel={handleCancelClick}
          />
        )
      }
    >
      <GenericModal
        display={containerState.showConfirmModal}
        title={'Confirm changes'}
        message={
          <>
            <div>If you cancel now, this Lease/License will not be saved.</div>
            <br />
            <strong>Are you sure you want to Cancel?</strong>
          </>
        }
        handleOk={handleCancelConfirm}
        handleCancel={() => setContainerState({ showConfirmModal: false })}
        okButtonText="Ok"
        cancelButtonText="Resume editing"
        show
      />
      <StyledFormWrapper>
        <ViewSelector
          formikRef={formikRef}
          lease={lease}
          refreshLease={refresh}
          setLease={setLease}
          isEditing={containerState.isEditing}
          activeEditForm={containerState.activeEditForm}
          activeTab={containerState.activeTab}
          setContainerState={setContainerState}
        />
      </StyledFormWrapper>
    </MapSideBarLayout>
  );
};

export default LeaseContainer;

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 2rem;
  padding-bottom: 1rem;
`;
