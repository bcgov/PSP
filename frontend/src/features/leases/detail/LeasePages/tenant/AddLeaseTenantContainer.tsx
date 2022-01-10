import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { useUpdateLease } from 'features/leases/hooks/useUpdateLease';
import { apiLeaseToFormLease, formLeaseToApiLease } from 'features/leases/leaseUtils';
import { FormikProps } from 'formik';
import { IContactSearchResult, IFormLease } from 'interfaces';
import * as React from 'react';
import { useContext } from 'react';
import { useState } from 'react';
import { useHistory } from 'react-router';

import AddLeaseTenantForm from './AddLeaseTenantForm';

interface IAddLeaseTenantContainerProps {}

export const AddLeaseTenantContainer: React.FunctionComponent<IAddLeaseTenantContainerProps> = () => {
  const formikRef = React.useRef<FormikProps<IFormLease>>(null);
  const { lease, setLease } = useContext(LeaseStateContext);
  const [selectedTenants, setSelectedTenants] = useState<IContactSearchResult[]>([]);
  const { updateLease } = useUpdateLease();
  const history = useHistory();

  const onCancel = () => {
    history.push(`/lease/${lease?.id}/tenant`);
  };

  const onSubmit = async (lease: IFormLease) => {
    try {
      const leaseToUpdate = formLeaseToApiLease(lease);
      const updatedLease = await updateLease(leaseToUpdate, undefined, undefined, 'tenants');
      if (!!updatedLease?.id) {
        formikRef?.current?.resetForm({ values: apiLeaseToFormLease(updatedLease) });
        setLease(updatedLease);
        history.push(`/lease/${updatedLease?.id}/tenant`);
      }
    } finally {
      formikRef?.current?.setSubmitting(false);
    }
  };

  return (
    <>
      <AddLeaseTenantForm
        initialValues={apiLeaseToFormLease(lease)}
        selectedTenants={selectedTenants}
        setSelectedTenants={setSelectedTenants}
        onCancel={onCancel}
        onSubmit={onSubmit}
        formikRef={formikRef}
      />
    </>
  );
};

export default AddLeaseTenantContainer;
