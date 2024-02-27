import { ENVIRONMENT } from '@/constants';
import CustomAxios from '@/customAxios';
import { ApiGen_Concepts_LeaseTerm } from '@/models/api/generated/ApiGen_Concepts_LeaseTerm';

export const getLeaseTerms = (leaseId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_LeaseTerm[]>(
    `/leases/${leaseId}/terms`,
  );
export const deleteLeaseTerm = (term: ApiGen_Concepts_LeaseTerm) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(`/leases/${term.leaseId}/terms`, {
    data: term,
  });
export const putLeaseTerm = (term: ApiGen_Concepts_LeaseTerm) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_LeaseTerm>(
    `/leases/${term.leaseId}/terms/${term.id}`,
    term,
  );
export const postLeaseTerm = (term: ApiGen_Concepts_LeaseTerm) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<ApiGen_Concepts_LeaseTerm>(
    `/leases/${term.leaseId}/terms`,
    term,
  );
