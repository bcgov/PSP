import { ENVIRONMENT } from '@/constants';
import CustomAxios from '@/customAxios';
import { ApiGen_Concepts_LeaseTenant } from '@/models/api/generated/ApiGen_Concepts_LeaseTenant';

export const updateLeaseTenants = (leaseId: number, improvements: ApiGen_Concepts_LeaseTenant[]) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_LeaseTenant[]>(
    `/leases/${leaseId}/tenants`,
    improvements,
  );

export const getLeaseTenants = (leaseId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_LeaseTenant[]>(
    `/leases/${leaseId}/tenants`,
  );
