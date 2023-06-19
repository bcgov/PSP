import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { LeaseFormModel } from 'features/leases/models';
import { FormikProps } from 'formik';
import { Api_LeaseTenant } from 'models/api/LeaseTenant';
import * as React from 'react';

import ProtectedComponent from '@/components/common/ProtectedComponent';
import { Claims } from '@/constants/claims';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import { IFormLease } from '@/interfaces';

import { AddLeaseTenantContainer } from './AddLeaseTenantContainer';
import AddLeaseTenantForm from './AddLeaseTenantForm';
import { FormTenant } from './models';
import { ViewTenantForm } from './ViewTenantForm';

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
