import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { ApiGen_Concepts_Payment } from '@/models/api/generated/ApiGen_Concepts_Payment';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import {
  deleteLeasePayment,
  postLeasePayment,
  putLeasePayment,
} from '../pims-api/useApiLeasePayments';
import { useApiRequestWrapper } from '../util/useApiRequestWrapper';

/**
 * hook that interacts with the lease payment API.
 */
export const useLeasePaymentRepository = () => {
  const updateLeasePaymentApi = useApiRequestWrapper<
    (
      leaseId: number,
      payment: ApiGen_Concepts_Payment,
    ) => Promise<AxiosResponse<ApiGen_Concepts_Payment, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, payment: ApiGen_Concepts_Payment) =>
        await putLeasePayment(leaseId, payment),
      [],
    ),
    requestName: 'putLeasePayment',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const addLeasePaymentApi = useApiRequestWrapper<
    (
      leaseId: number,
      payment: ApiGen_Concepts_Payment,
    ) => Promise<AxiosResponse<ApiGen_Concepts_Payment, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, payment: ApiGen_Concepts_Payment) =>
        await postLeasePayment(leaseId, payment),
      [],
    ),
    requestName: 'postLeasePayment',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const deleteLeasePaymentApi = useApiRequestWrapper<
    (leaseId: number, payment: ApiGen_Concepts_Payment) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, payment: ApiGen_Concepts_Payment) =>
        await deleteLeasePayment(leaseId, payment),
      [],
    ),
    requestName: 'deleteLeasePayment',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  return useMemo(
    () => ({
      updateLeasePayment: updateLeasePaymentApi,
      addLeasePayment: addLeasePaymentApi,
      deleteLeasePayment: deleteLeasePaymentApi,
    }),
    [updateLeasePaymentApi, addLeasePaymentApi, deleteLeasePaymentApi],
  );
};
