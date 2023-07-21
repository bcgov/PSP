import { ENVIRONMENT } from '@/constants';
import CustomAxios from '@/customAxios';
import { Api_LeasePayment } from '@/models/api/LeasePayment';

export const deleteLeasePayment = (leaseId: number, payment: Api_LeasePayment) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(`/leases/${leaseId}/payment`, {
    data: payment,
  });
export const putLeasePayment = (leaseId: number, payment: Api_LeasePayment) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_LeasePayment>(
    `/leases/${leaseId}/payment/${payment.id}`,
    payment,
  );
export const postLeasePayment = (leaseId: number, payment: Api_LeasePayment) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<Api_LeasePayment>(
    `/leases/${leaseId}/payment`,
    payment,
  );
