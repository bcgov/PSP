import { ILease } from 'interfaces';
import React from 'react';

import { ILeasePayment } from './../../interfaces/ILeasePayment';
import { useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the lease payment endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiLeasePayments = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      deleteLeasePayment: (payment: ILeasePayment) =>
        api.delete<ILease>(`/leases/${payment.leaseId}/payment`, { data: payment }),
      putLeasePayment: (payment: ILeasePayment) =>
        api.put<ILease>(`/leases/${payment.leaseId}/payment/${payment.id}`, payment),
      postLeasePayment: (payment: ILeasePayment) =>
        api.post<ILease>(`/leases/${payment.leaseId}/payment`, payment),
    }),
    [api],
  );
};
