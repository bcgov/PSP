import { FormikProps } from 'formik';
import { useContext, useEffect } from 'react';

import ProtectedComponent from '@/components/common/ProtectedComponent';
import { Claims } from '@/constants';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import { useLeaseTenantRepository } from '@/hooks/repositories/useLeaseTenantRepository';
import { Api_LeaseTenant } from '@/models/api/LeaseTenant';

import AddLeaseTenantContainer from './AddLeaseTenantContainer';
import AddLeaseTenantForm from './AddLeaseTenantForm';
import { FormTenant } from './models';
import ViewTenantForm from './ViewTenantForm';

const TenantContainer: React.FunctionComponent<React.PropsWithChildren<LeasePageProps>> = ({
  isEditing,
  formikRef,
  onEdit,
}) => {
  const { lease } = useContext(LeaseStateContext);
  const {
    getLeaseTenants: { execute: getLeaseTenants, loading, response: tenants },
  } = useLeaseTenantRepository();
  useEffect(() => {
    lease?.id && getLeaseTenants(lease.id);
  }, [lease, getLeaseTenants]);

  const formTenants = tenants?.map((t: Api_LeaseTenant) => new FormTenant(t)) ?? [];

  return !!isEditing ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <AddLeaseTenantContainer
        formikRef={formikRef as React.RefObject<FormikProps<LeaseFormModel>>}
        onEdit={onEdit}
        tenants={formTenants}
        View={AddLeaseTenantForm}
      />
    </ProtectedComponent>
  ) : (
    <ViewTenantForm tenants={formTenants} loading={loading} />
  );
};

export default TenantContainer;
