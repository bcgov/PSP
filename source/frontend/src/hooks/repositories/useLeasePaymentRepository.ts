import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { Api_LeasePayment } from '@/models/api/LeasePayment';
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
    (leaseId: number, payment: Api_LeasePayment) => Promise<AxiosResponse<Api_LeasePayment, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, payment: Api_LeasePayment) => await putLeasePayment(leaseId, payment),
      [],
    ),
    requestName: 'putLeasePayment',
    onSuccess: useAxiosSuccessHandler('payment saved successfully.'),
    onError: useAxiosErrorHandler(),
  });

  const addLeasePaymentApi = useApiRequestWrapper<
    (leaseId: number, payment: Api_LeasePayment) => Promise<AxiosResponse<Api_LeasePayment, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, payment: Api_LeasePayment) =>
        await postLeasePayment(leaseId, payment),
      [],
    ),
    requestName: 'postLeasePayment',
    onSuccess: useAxiosSuccessHandler('payment saved successfully.'),
    onError: useAxiosErrorHandler(),
  });

  const deleteLeasePaymentApi = useApiRequestWrapper<
    (leaseId: number, payment: Api_LeasePayment) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, payment: Api_LeasePayment) =>
        await deleteLeasePayment(leaseId, payment),
      [],
    ),
    requestName: 'deleteLeasePayment',
    onSuccess: useAxiosSuccessHandler('payment deleted successfully.'),
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
