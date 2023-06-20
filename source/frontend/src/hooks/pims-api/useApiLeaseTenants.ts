import { ENVIRONMENT } from '@/constants';
import CustomAxios from '@/customAxios';
import { Api_LeaseTenant } from '@/models/api/LeaseTenant';

export const updateLeaseTenants = (leaseId: number, improvements: Api_LeaseTenant[]) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_LeaseTenant[]>(
    `/leases/${leaseId}/tenants`,
    improvements,
  );

export const getLeaseTenants = (leaseId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_LeaseTenant[]>(`/leases/${leaseId}/tenants`);
