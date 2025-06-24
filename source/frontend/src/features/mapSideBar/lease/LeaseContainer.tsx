import { FormikProps } from 'formik';
import React, {
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useReducer,
  useRef,
  useState,
} from 'react';
import { useHistory } from 'react-router-dom';
import * as Yup from 'yup';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LocationBoundaryDataset } from '@/components/common/mapFSM/models';
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
import PeriodPaymentsView from '@/features/leases/detail/LeasePages/payment/table/periods/PaymentPeriodsView';
import LeaseStakeholderContainer from '@/features/leases/detail/LeasePages/stakeholders/LeaseStakeholderContainer';
import Surplus from '@/features/leases/detail/LeasePages/surplus/Surplus';
import { isLeaseFile, LeaseFormModel } from '@/features/leases/models';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { useQuery } from '@/hooks/use-query';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists, filePropertyToLocationBoundaryDataset } from '@/utils';

import { SideBarContext } from '../context/sidebarContext';
import usePathGenerator from '../shared/sidebarPathGenerator';
import { LeaseFileTabNames } from './detail/LeaseFileTabs';
import { ILeaseViewProps } from './LeaseView';
import LeaseRouter from './tabs/LeaseRouter';

export interface ILeaseContainerProps {
  leaseId: number;
  onClose?: () => void;
  View: React.FunctionComponent<ILeaseViewProps>;
}

// Interface for our internal state
export interface LeaseContainerState {
  isEditing: boolean;
  activeEditForm?: LeasePageNames;
  activeTab?: LeaseFileTabNames;
}

const initialState: LeaseContainerState = {
  isEditing: false,
  activeEditForm: undefined,
  activeTab: undefined,
};

export interface LeasePageProps<T> {
  isEditing: boolean;
  onEdit?: (isEditing: boolean) => void;
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
  onSuccess: () => void;
  refreshLease?: () => void;
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
    },
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

export const LeaseContainer: React.FC<ILeaseContainerProps> = ({ leaseId, onClose, View }) => {
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
  const { setLease, getCompleteLease, refresh, loading } = useLeaseDetail(leaseId);
  const {
    file,
    staleFile,
    setStaleLastUpdatedBy,
    setLastUpdatedBy,
    staleLastUpdatedBy,
    lastUpdatedBy,
  } = useContext(SideBarContext);

  const query = useQuery();
  const history = useHistory();
  const [isValid, setIsValid] = useState<boolean>(true);
  const { setModalContent, setDisplayModal } = useModalContext();

  const pathSolver = usePathGenerator();

  const activeTab = containerState.activeTab;
  const { setFullWidthSideBar, setFilePropertyLocations } = useMapStateMachine();

  const {
    getLastUpdatedBy: { execute: getLastUpdatedBy, loading: getLastUpdatedByLoading },
  } = useLeaseRepository();

  const lease: ApiGen_Concepts_Lease | null = isLeaseFile(file) ? file : null;

  const locations: LocationBoundaryDataset[] = useMemo(() => {
    if (exists(lease?.fileProperties)) {
      return lease?.fileProperties
        .map(leaseProp => filePropertyToLocationBoundaryDataset(leaseProp))
        .filter(exists);
    } else {
      return [];
    }
  }, [lease?.fileProperties]);

  const onChildSuccess = useCallback(() => {
    setStaleLastUpdatedBy(true);
  }, [setStaleLastUpdatedBy]);

  const handleCancelConfirm = () => {
    if (formikRef !== undefined) {
      formikRef.current?.resetForm();
    }
    setIsValid(true);
    setContainerState({
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

  const handleCancelClick = (onCancelConfirm?: () => void) => {
    if (formikRef !== undefined) {
      if (formikRef.current?.dirty) {
        setModalContent({
          ...getCancelModalProps(),
          handleOk: () => {
            handleCancelConfirm();
            setDisplayModal(false);
            if (typeof onCancelConfirm === 'function') {
              onCancelConfirm();
            }
          },
          handleCancel: () => setDisplayModal(false),
        });
        setDisplayModal(true);
      } else {
        handleCancelConfirm();
      }
    } else {
      handleCancelConfirm();
    }

    setIsPropertyEditing(false);
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
    if (!exists(lease) || leaseId !== lease.id || staleFile) {
      getCompleteLease();
    }
  }, [staleFile, leaseId, lease, getCompleteLease]);

  useEffect(() => {
    if (lastUpdatedBy === undefined || leaseId !== lastUpdatedBy?.parentId || staleLastUpdatedBy) {
      fetchLastUpdatedBy();
    }
  }, [fetchLastUpdatedBy, lastUpdatedBy, leaseId, staleLastUpdatedBy]);

  useEffect(() => {
    setFilePropertyLocations(locations);
  }, [setFilePropertyLocations, locations]);

  const onSelectFileSummary = () => {
    if (!exists(lease)) {
      return;
    }

    if (containerState.isEditing) {
      if (formikRef?.current?.dirty) {
        handleCancelClick(() => pathSolver.showFile('lease', lease?.id ?? 0));
      }
    }
    pathSolver.showFile('lease', lease?.id ?? 0);
  };

  const onSelectProperty = (filePropertyId: number) => {
    if (!exists(lease)) {
      return;
    }

    if (containerState.isEditing) {
      if (formikRef?.current?.dirty) {
        handleCancelClick(() =>
          pathSolver.showFilePropertyId('lease', lease?.id ?? 0, filePropertyId),
        );
        return;
      }
    }

    // The index needs to be offset to match the menu index
    pathSolver.showFilePropertyId('lease', lease?.id ?? 0, filePropertyId);
  };

  const onEditProperties = () => {
    if (exists(lease)) {
      pathSolver.editProperties('lease', lease.id ?? 0);
    }
  };

  const setIsPropertyEditing = useCallback(
    (value: boolean) => {
      if (value) {
        query.set('edit', value.toString());
      } else {
        query.delete('edit');
      }

      setContainerState({
        isEditing: value,
      });
      history.push({ search: query.toString() });
    },
    [history, query],
  );

  useEffect(() => {
    setIsPropertyEditing(query.get('edit') === 'true');
  }, [query, setIsPropertyEditing]);

  const onPropertyUpdate = () => {
    setIsPropertyEditing(false);
    refresh();
  };

  // UI components
  if (loading || getLastUpdatedByLoading) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <View
      setIsEditing={setIsPropertyEditing}
      onClose={close}
      onSave={handleSaveClick}
      onCancel={handleCancelClick}
      onSelectFileSummary={onSelectFileSummary}
      onSelectProperty={onSelectProperty}
      onEditProperties={onEditProperties}
      onPropertyUpdateSuccess={onPropertyUpdate}
      onChildSuccess={onChildSuccess}
      refreshLease={refresh}
      setLease={setLease}
      formikRef={formikRef}
      containerState={containerState}
      setContainerState={setContainerState}
      isFormValid={isValid}
      lease={lease}
    ></View>
  );
};

export default LeaseContainer;
