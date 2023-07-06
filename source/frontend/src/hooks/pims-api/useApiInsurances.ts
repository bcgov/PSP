import { ENVIRONMENT } from '@/constants';
import CustomAxios from '@/customAxios';
import { Api_Insurance } from '@/models/api/Insurance';

export const updateLeaseInsurances = (leaseId: number, insurances: Api_Insurance[]) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_Insurance[]>(
    `/leases/${leaseId}/insurances`,
    insurances,
  );

export const getLeaseInsurances = (leaseId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<[]>(`/leases/${leaseId}/insurances`);
