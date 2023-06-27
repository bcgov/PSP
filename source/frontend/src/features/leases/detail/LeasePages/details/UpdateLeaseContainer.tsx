import { FormikProps } from 'formik/dist/types';
import * as React from 'react';
import { useEffect } from 'react';
import { useContext } from 'react';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { useLeaseDetail } from '@/features/leases/hooks/useLeaseDetail';
import { useUpdateLease } from '@/features/leases/hooks/useUpdateLease';
import { LeaseFormModel } from '@/features/leases/models';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { Api_Lease } from '@/models/api/Lease';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

import { IUpdateLeaseFormProps } from './UpdateLeaseForm';

export interface UpdateLeaseContainerProps {
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
  onEdit: (isEditing: boolean) => void;
  View: React.FunctionComponent<React.PropsWithChildren<IUpdateLeaseFormProps>>;
}

export const UpdateLeaseContainer: React.FunctionComponent<
  React.PropsWithChildren<UpdateLeaseContainerProps>
> = ({ formikRef, onEdit, View }) => {
  const { lease } = useContext(LeaseStateContext);
  const {
    getApiLeaseById: { execute, response: apiLease, loading },
    refresh,
  } = useLeaseDetail(lease?.id ?? undefined);
  const { updateApiLease } = useUpdateLease();
  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to update Lease File');

  const leaseId = lease?.id;
  useEffect(() => {
    const exec = async () => {
      if (leaseId) {
        var lease = await execute(leaseId);
        formikRef?.current?.resetForm({ values: LeaseFormModel.fromApi(lease) });
      }
    };
    exec();
  }, [execute, leaseId, formikRef]);

  const onSubmit = async (lease: LeaseFormModel, userOverrideCodes: UserOverrideCode[] = []) => {
    try {
      const leaseToUpdate = LeaseFormModel.toApi(lease);

      const updatedLease = await updateApiLease.execute(leaseToUpdate, userOverrideCodes);
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
      <View
        onSubmit={(lease: LeaseFormModel) =>
          withUserOverride((userOverrideCodes: UserOverrideCode[]) =>
            onSubmit(lease, userOverrideCodes),
          )
        }
        initialValues={initialValues}
        formikRef={formikRef}
      />
    </>
  );
};

export default UpdateLeaseContainer;
