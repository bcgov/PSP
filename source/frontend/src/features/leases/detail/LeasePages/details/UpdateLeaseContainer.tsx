import { FormikProps } from 'formik/dist/types';
import { useContext, useEffect } from 'react';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { useLeaseDetail } from '@/features/leases/hooks/useLeaseDetail';
import { useUpdateLease } from '@/features/leases/hooks/useUpdateLease';
import { LeaseFormModel } from '@/features/leases/models';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { isValidId } from '@/utils';

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
  const { getCompleteLease, refresh, loading } = useLeaseDetail(lease?.id ?? undefined);
  const { updateApiLease } = useUpdateLease();
  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to update Lease File');

  const mapMachine = useMapStateMachine();

  const leaseId = lease?.id;
  useEffect(() => {
    const exec = async () => {
      if (leaseId) {
        const lease = await getCompleteLease();
        formikRef?.current?.resetForm({ values: LeaseFormModel.fromApi(lease) });
      }
    };
    exec();
  }, [getCompleteLease, leaseId, formikRef]);

  const onSubmit = async (lease: LeaseFormModel, userOverrideCodes: UserOverrideCode[] = []) => {
    try {
      const leaseToUpdate = LeaseFormModel.toApi(lease);

      const updatedLease = await updateApiLease.execute(leaseToUpdate, userOverrideCodes);
      afterSubmit(updatedLease);
    } finally {
      formikRef?.current?.setSubmitting(false);
    }
  };

  const afterSubmit = async (updatedLease?: ApiGen_Concepts_Lease) => {
    if (isValidId(updatedLease?.id)) {
      formikRef?.current?.resetForm({ values: formikRef?.current?.values });
      await refresh();
      mapMachine.refreshMapProperties();
      onEdit(false);
    }
  };

  return (
    <>
      <LoadingBackdrop show={loading} parentScreen></LoadingBackdrop>
      <View
        onSubmit={(lease: LeaseFormModel) =>
          withUserOverride((userOverrideCodes: UserOverrideCode[]) =>
            onSubmit(lease, userOverrideCodes),
          )
        }
        formikRef={formikRef}
      />
    </>
  );
};

export default UpdateLeaseContainer;
