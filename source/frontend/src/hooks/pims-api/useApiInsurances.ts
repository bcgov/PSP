import { ENVIRONMENT } from '@/constants';
import CustomAxios from '@/customAxios';
import { ApiGen_Concepts_Insurance } from '@/models/api/generated/ApiGen_Concepts_Insurance';

export const updateLeaseInsurances = (leaseId: number, insurances: ApiGen_Concepts_Insurance[]) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_Insurance[]>(
    `/leases/${leaseId}/insurances`,
    insurances,
  );

export const getLeaseInsurances = (leaseId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<[]>(`/leases/${leaseId}/insurances`);
