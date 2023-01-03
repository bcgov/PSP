import GenericModal from 'components/common/GenericModal';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { useLeaseDetail } from 'features/leases/hooks/useLeaseDetail';
import { useUpdateLease } from 'features/leases/hooks/useUpdateLease';
import { LeaseFormModel } from 'features/leases/models';
import { FormikProps } from 'formik/dist/types';
import { Api_Lease } from 'models/api/Lease';
import * as React from 'react';
import { useState } from 'react';
import { useEffect } from 'react';
import { useContext } from 'react';

import { UpdateLeaseForm } from './UpdateLeaseForm';

interface IAddLeaseParams {
  lease?: Api_Lease;
  userOverride?: string;
}

interface UpdateLeaseContainerProps {
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
  onEdit: (isEditing: boolean) => void;
}

export const UpdateLeaseContainer: React.FunctionComponent<
  React.PropsWithChildren<UpdateLeaseContainerProps>
> = ({ formikRef, onEdit }) => {
  const { lease } = useContext(LeaseStateContext);
  const {
    getApiLeaseById: { execute, response: apiLease, loading },
    refresh,
  } = useLeaseDetail(lease?.id);
  const [addLeaseParams, setAddLeaseParams] = useState<IAddLeaseParams | undefined>();
  const { updateApiLease } = useUpdateLease();

  const leaseId = lease?.id;
  //TODO: For now we make a duplicate request here for the lease in the newer format. In the future all lease pages will use the new format so this will no longer be necessary.
  useEffect(() => {
    const exec = async () => {
      if (leaseId) {
        var lease = await execute(leaseId);
        formikRef?.current?.resetForm({ values: LeaseFormModel.fromApi(lease) });
      }
    };
    exec();
  }, [execute, leaseId, formikRef]);

  const onSubmit = async (lease: LeaseFormModel) => {
    try {
      const leaseToUpdate = lease.toApi();

      const updatedLease = await updateApiLease(leaseToUpdate, (userOverrideMessage?: string) =>
        setAddLeaseParams({ lease: leaseToUpdate, userOverride: userOverrideMessage }),
      );
      afterSubmit(updatedLease);
    } finally {
      formikRef?.current?.setSubmitting(false);
    }
  };

  const afterSubmit = async (updatedLease?: Api_Lease) => {
    if (!!updatedLease?.id) {
      formikRef?.current?.resetForm({ values: formikRef?.current?.values });
      await refresh();
      onEdit(false);
    }
  };

  const initialValues = LeaseFormModel.fromApi(apiLease);
  return (
    <>
      <LoadingBackdrop show={loading} parentScreen></LoadingBackdrop>
      <UpdateLeaseForm onSubmit={onSubmit} initialValues={initialValues} formikRef={formikRef} />
      <GenericModal
        title="Warning"
        display={!!addLeaseParams}
        message={addLeaseParams?.userOverride}
        handleOk={async () => {
          if (!!addLeaseParams?.lease) {
            const leaseResponse = await updateApiLease(addLeaseParams.lease, undefined, true);
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
