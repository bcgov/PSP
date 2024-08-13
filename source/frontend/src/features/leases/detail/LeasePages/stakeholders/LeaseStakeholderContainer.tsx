import { FormikProps } from 'formik/dist/types';
import { useContext, useEffect } from 'react';

import ProtectedComponent from '@/components/common/ProtectedComponent';
import { Claims } from '@/constants';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import { useLeaseStakeholderRepository } from '@/hooks/repositories/useLeaseStakeholderRepository';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';

import AddLeaseStakeholderContainer from './AddLeaseStakeholderContainer';
import AddLeaseStakeholderForm from './AddLeaseStakeholderForm';
import { FormStakeholder } from './models';
import ViewTenantForm from './ViewStakeholderForm';

const TenantContainer: React.FunctionComponent<React.PropsWithChildren<LeasePageProps<void>>> = ({
  isEditing,
  formikRef,
  onEdit,
  onSuccess,
}) => {
  const { lease } = useContext(LeaseStateContext);
  const getIsPayableLease = () => {
    return lease?.paymentReceivableType.id !== 'RCVBL' ? true : false;
  };
  const {
    getLeaseStakeholders: { execute: getLeaseStakeholders, loading, response: stakeholders },
  } = useLeaseStakeholderRepository();
  useEffect(() => {
    lease?.id && getLeaseStakeholders(lease.id);
  }, [lease, getLeaseStakeholders]);

  const formStakeholders =
    stakeholders?.map((t: ApiGen_Concepts_LeaseStakeholder) => new FormStakeholder(t)) ?? [];

  return isEditing ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <AddLeaseStakeholderContainer
        formikRef={formikRef as React.RefObject<FormikProps<LeaseFormModel>>}
        onEdit={onEdit}
        stakeholders={formStakeholders}
        View={AddLeaseStakeholderForm}
        onSuccess={onSuccess}
        isPayableLease={getIsPayableLease()}
      />
    </ProtectedComponent>
  ) : (
    <ViewTenantForm stakeholders={formStakeholders} loading={loading} />
  );
};

export default TenantContainer;
