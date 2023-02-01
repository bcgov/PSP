import ProtectedComponent from 'components/common/ProtectedComponent';
import { Claims } from 'constants/claims';
import { LeasePageProps } from 'features/properties/map/lease/LeaseContainer';
import { FormikProps } from 'formik';
import { IFormLease } from 'interfaces';
import * as React from 'react';

import { AddLeaseTenantContainer } from './AddLeaseTenantContainer';
import AddLeaseTenantForm from './AddLeaseTenantForm';
import { ViewTenantForm } from './ViewTenantForm';

const TenantContainer: React.FunctionComponent<React.PropsWithChildren<LeasePageProps>> = ({
  isEditing,
  formikRef,
  onEdit,
}) => {
  return !!isEditing ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <AddLeaseTenantContainer
        formikRef={formikRef as React.RefObject<FormikProps<IFormLease>>}
        onEdit={onEdit}
        View={AddLeaseTenantForm}
      />
    </ProtectedComponent>
  ) : (
    <ViewTenantForm />
  );
};

export default TenantContainer;
