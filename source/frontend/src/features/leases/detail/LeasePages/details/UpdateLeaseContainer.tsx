import { useMapSearch } from 'components/maps/hooks/useMapSearch';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { useLeaseDetail } from 'features/leases/hooks/useLeaseDetail';
import { useUpdateLease } from 'features/leases/hooks/useUpdateLease';
import { LeaseFormModel } from 'features/leases/models';
import { FormikProps } from 'formik/dist/types';
import useApiUserOverride from 'hooks/useApiUserOverride';
import { Api_Lease } from 'models/api/Lease';
import { UserOverrideCode } from 'models/api/UserOverrideCode';
import * as React from 'react';
import { useEffect } from 'react';
import { useContext } from 'react';

import { UpdateLeaseForm } from './UpdateLeaseForm';

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
  const { updateApiLease } = useUpdateLease();
  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to update Lease File');
  const { search } = useMapSearch();

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

  const onSubmit = async (lease: LeaseFormModel, userOverrideCodes: UserOverrideCode[] = []) => {
    try {
      const leaseToUpdate = lease.toApi();

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
      await search();
      onEdit(false);
    }
  };

  const initialValues = LeaseFormModel.fromApi(apiLease);
  return (
    <>
      <LoadingBackdrop show={loading} parentScreen></LoadingBackdrop>
      <UpdateLeaseForm
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
