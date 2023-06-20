import { ENVIRONMENT } from '@/constants';
import CustomAxios from '@/customAxios';
import { Api_LeaseTerm } from '@/models/api/LeaseTerm';

export const getLeaseTerms = (leaseId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_LeaseTerm[]>(`/leases/${leaseId}/term`);
export const deleteLeaseTerm = (term: Api_LeaseTerm) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(`/leases/${term.leaseId}/term`, {
    data: term,
  });
export const putLeaseTerm = (term: Api_LeaseTerm) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_LeaseTerm>(
    `/leases/${term.leaseId}/term/${term.id}`,
    term,
  );
export const postLeaseTerm = (term: Api_LeaseTerm) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<Api_LeaseTerm>(
    `/leases/${term.leaseId}/term`,
    term,
  );
