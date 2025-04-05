import { AxiosError } from 'axios';
import { FormikProps } from 'formik/dist/types';
import { useCallback, useContext } from 'react';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import * as API from '@/constants/API';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { useLeaseDetail } from '@/features/leases/hooks/useLeaseDetail';
import { useUpdateLease } from '@/features/leases/hooks/useUpdateLease';
import { LeaseFormModel } from '@/features/leases/models';
import { LeaseStatusUpdateSolver } from '@/features/leases/models/LeaseStatusUpdateSolver';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useModalContext } from '@/hooks/useModalContext';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { isValidId } from '@/utils';

import { IUpdateLeaseFormProps } from './UpdateLeaseForm';

export interface UpdateLeaseContainerProps {
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
  onEdit: (isEditing: boolean) => void;
  View: React.FunctionComponent<IUpdateLeaseFormProps>;
}

export const UpdateLeaseContainer: React.FunctionComponent<UpdateLeaseContainerProps> = ({
  formikRef,
  onEdit,
  View,
}) => {
  const { lease } = useContext(LeaseStateContext);
  const { getCompleteLease, refresh, loading } = useLeaseDetail(lease?.id ?? undefined);
  const { updateApiLease } = useUpdateLease();
  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to update Lease File');
  const { getByType } = useLookupCodeHelpers();
  const { setModalContent, setDisplayModal } = useModalContext();
  const consultationTypes = getByType(API.CONSULTATION_TYPES);
  const leaseStatusTypes = getByType(API.LEASE_STATUS_TYPES);

  // Not all consultations might be coming from the backend. Add the ones missing.

  const leaseId = lease?.id;

  const refreshCompleteLease = useCallback(
    async (leaseId?: number) => {
      if (isValidId(leaseId)) {
        const lease = await getCompleteLease();
        formikRef?.current?.resetForm({ values: LeaseFormModel.fromApi(lease) });
      }
    },
    [formikRef, getCompleteLease],
  );

  useDeepCompareEffect(() => {
    const exec = async () => {
      await refreshCompleteLease(leaseId);
    };
    exec();
  }, [leaseId, consultationTypes, refreshCompleteLease]);

  const afterSubmit = useCallback(
    async (updatedLease?: ApiGen_Concepts_Lease) => {
      if (isValidId(updatedLease?.id)) {
        formikRef?.current?.resetForm({ values: formikRef?.current?.values });
        await refresh();
        onEdit(false);
      }
    },
    [formikRef, onEdit, refresh],
  );

  const onSubmit = useCallback(
    async (lease: LeaseFormModel, userOverrideCodes: UserOverrideCode[] = []) => {
      try {
        const leaseToUpdate = LeaseFormModel.toApi(lease);

        const updatedLease = await updateApiLease.execute(leaseToUpdate, userOverrideCodes);
        afterSubmit(updatedLease);
      } finally {
        formikRef?.current?.setSubmitting(false);
      }
    },
    [afterSubmit, formikRef, updateApiLease],
  );

  const customErrorHandler = (e: AxiosError<IApiError>) => {
    if (e?.response?.data?.type === 'BusinessRuleViolationException') {
      setModalContent({
        title: 'Warning',
        message: e.response.data.error,
        okButtonText: 'Close',
        variant: 'error',
        handleOk: async () => {
          await refreshCompleteLease(leaseId);
          setDisplayModal(false);
        },
      });
      setDisplayModal(true);
    } else {
      throw e;
    }
  };

  return (
    <>
      <LoadingBackdrop show={loading} parentScreen></LoadingBackdrop>
      <View
        onSubmit={async (values: LeaseFormModel) => {
          const updatedFile = LeaseFormModel.toApi(values);
          const currentFileSolver = new LeaseStatusUpdateSolver(lease.fileStatusTypeCode);
          const updatedFileSolver = new LeaseStatusUpdateSolver(updatedFile.fileStatusTypeCode);

          if (!currentFileSolver.isAdminProtected() && updatedFileSolver.isAdminProtected()) {
            const statusCode = leaseStatusTypes.find(x => x.id === values.statusTypeCode);
            setModalContent({
              variant: 'info',
              title: 'Confirm status change',
              message: (
                <>
                  <p>
                    You marked this file as {statusCode.name}. If you save it, only the
                    administrator can turn it back on. You will still see it in the management
                    table.
                  </p>
                  <p>Do you want to acknowledge and proceed?</p>
                </>
              ),
              okButtonText: 'Yes',
              cancelButtonText: 'No',
              handleOk: async () => {
                await withUserOverride(
                  (userOverrideCodes: UserOverrideCode[]) => onSubmit(values, userOverrideCodes),
                  [],
                  customErrorHandler,
                );
              },
              handleCancel: () => setDisplayModal(false),
            });
            setDisplayModal(true);
          } else {
            await withUserOverride(
              (userOverrideCodes: UserOverrideCode[]) => onSubmit(values, userOverrideCodes),
              [],
              customErrorHandler,
            );
          }
        }}
        formikRef={formikRef}
        initialValues={LeaseFormModel.fromApi(lease)}
      />
    </>
  );
};

export default UpdateLeaseContainer;
