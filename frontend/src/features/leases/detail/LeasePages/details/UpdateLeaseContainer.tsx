import GenericModal from 'components/common/GenericModal';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { useUpdateLease } from 'features/leases/hooks/useUpdateLease';
import { addFormLeaseToApiLease, apiLeaseToAddFormLease } from 'features/leases/leaseUtils';
import { FormikProps } from 'formik/dist/types';
import { IAddFormLease, ILease } from 'interfaces';
import * as React from 'react';
import { useState } from 'react';
import { useContext, useMemo } from 'react';
import { useRef } from 'react';
import { useHistory } from 'react-router-dom';

import { UpdateLeaseForm } from './UpdateLeaseForm';

interface IAddLeaseParams {
  lease?: ILease;
  userOverride?: string;
}

export const UpdateLeaseContainer: React.FunctionComponent = props => {
  const { lease, setLease } = useContext(LeaseStateContext);
  const [addLeaseParams, setAddLeaseParams] = useState<IAddLeaseParams | undefined>();
  const { updateLease } = useUpdateLease();
  const history = useHistory();
  const addFormLease = useMemo(() => apiLeaseToAddFormLease(lease), [lease]);
  const formikRef = useRef<FormikProps<IAddFormLease>>(null);

  const onSubmit = async (lease: IAddFormLease) => {
    try {
      const leaseToUpdate = addFormLeaseToApiLease(lease);
      const updatedLease = await updateLease(leaseToUpdate, (userOverrideMessage?: string) =>
        setAddLeaseParams({ lease: leaseToUpdate, userOverride: userOverrideMessage }),
      );
      afterSubmit(updatedLease);
    } finally {
      formikRef?.current?.setSubmitting(false);
    }
  };

  const afterSubmit = (updatedLease?: ILease) => {
    if (!!updatedLease?.id) {
      formikRef?.current?.resetForm({ values: apiLeaseToAddFormLease(updatedLease) });
      setLease(updatedLease);
      history.push(`/lease/${updatedLease?.id}`);
    }
  };

  return (
    <>
      <UpdateLeaseForm
        onCancel={() => history.push(`/lease/${lease?.id}`)}
        onSubmit={onSubmit}
        initialValues={addFormLease}
        formikRef={formikRef}
      />
      <GenericModal
        title="Warning"
        display={!!addLeaseParams}
        message={addLeaseParams?.userOverride}
        handleOk={async () => {
          if (!!addLeaseParams?.lease) {
            const leaseResponse = await updateLease(addLeaseParams.lease, undefined, true);
            afterSubmit(leaseResponse);
            setAddLeaseParams(undefined);
          }
        }}
        handleCancel={() => setAddLeaseParams(undefined)}
        okButtonText="Save Anyways"
        okButtonVariant="warning"
        cancelButtonText="Cancel"
      />
    </>
  );
};

export default UpdateLeaseContainer;
