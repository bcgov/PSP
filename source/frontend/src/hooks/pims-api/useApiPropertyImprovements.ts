import { ENVIRONMENT } from '@/constants';
import CustomAxios from '@/customAxios';
import { Api_PropertyImprovement } from '@/models/api/PropertyImprovement';

export const updatePropertyImprovements = (
  leaseId: number,
  improvements: Api_PropertyImprovement[],
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_PropertyImprovement[]>(
    `/leases/${leaseId}/improvements`,
    improvements,
  );

export const getPropertyImprovements = (leaseId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_PropertyImprovement[]>(
    `/leases/${leaseId}/improvements`,
  );
