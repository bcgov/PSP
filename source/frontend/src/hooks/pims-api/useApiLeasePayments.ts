import { ENVIRONMENT } from '@/constants';
import CustomAxios from '@/customAxios';
import { ApiGen_Concepts_Payment } from '@/models/api/generated/ApiGen_Concepts_Payment';

export const deleteLeasePayment = (leaseId: number, payment: ApiGen_Concepts_Payment) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(`/leases/${leaseId}/payment`, {
    data: payment,
  });
export const putLeasePayment = (leaseId: number, payment: ApiGen_Concepts_Payment) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_Payment>(
    `/leases/${leaseId}/payment/${payment.id}`,
    payment,
  );
export const postLeasePayment = (leaseId: number, payment: ApiGen_Concepts_Payment) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<ApiGen_Concepts_Payment>(
    `/leases/${leaseId}/payment`,
    payment,
  );
