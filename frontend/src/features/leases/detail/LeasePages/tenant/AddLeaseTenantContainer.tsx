import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { useUpdateLease } from 'features/leases/hooks/useUpdateLease';
import { apiLeaseToFormLease, formLeaseToApiLease } from 'features/leases/leaseUtils';
import { FormikProps } from 'formik';
import { defaultFormLease, IFormLease, ILease } from 'interfaces';
import * as React from 'react';
import { useContext } from 'react';
import { useState } from 'react';
import { useHistory } from 'react-router';

import AddLeaseTenantForm from './AddLeaseTenantForm';
import PrimaryContactWarningModal, {
  getOrgsWithNoPrimaryContact,
} from './PrimaryContactWarningModal';
import { FormTenant } from './Tenant';

interface IAddLeaseTenantContainerProps {}

export const AddLeaseTenantContainer: React.FunctionComponent<IAddLeaseTenantContainerProps> = () => {
  const formikRef = React.useRef<FormikProps<IFormLease>>(null);
  const { lease, setLease } = useContext(LeaseStateContext);
  const [selectedTenants, setSelectedTenants] = useState<FormTenant[]>([]);
  const [handleSubmit, setHandleSubmit] = useState<Function | undefined>(undefined);
  const { updateLease } = useUpdateLease();
  const history = useHistory();

  const onCancel = () => {
    history.push(`/lease/${lease?.id}/tenant`);
  };

  const submit = async (leaseToUpdate: ILease) => {
    try {
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

  const onSubmit = async (lease: IFormLease) => {
    const leaseToUpdate = formLeaseToApiLease(lease);
    if (getOrgsWithNoPrimaryContact(lease)?.length > 0) {
      setHandleSubmit(() => () => submit(leaseToUpdate));
    } else {
      submit(leaseToUpdate);
    }
  };

  return (
    <>
      <AddLeaseTenantForm
        initialValues={{ ...defaultFormLease, ...apiLeaseToFormLease(lease) }}
        selectedTenants={selectedTenants}
        setSelectedTenants={setSelectedTenants}
        onCancel={onCancel}
        onSubmit={onSubmit}
        formikRef={formikRef}
      />
      <PrimaryContactWarningModal
        display={handleSubmit}
        setDisplay={setHandleSubmit}
        onCancel={() => setHandleSubmit(undefined)}
        lease={formikRef?.current?.values}
      />
    </>
  );
};

export default AddLeaseTenantContainer;
