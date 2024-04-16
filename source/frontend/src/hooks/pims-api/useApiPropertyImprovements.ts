import { ENVIRONMENT } from '@/constants';
import CustomAxios from '@/customAxios';
import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';

export const updatePropertyImprovements = (
  leaseId: number,
  improvements: ApiGen_Concepts_PropertyImprovement[],
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_PropertyImprovement[]>(
    `/leases/${leaseId}/improvements`,
    improvements,
  );

export const getPropertyImprovements = (leaseId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_PropertyImprovement[]>(
    `/leases/${leaseId}/improvements`,
  );
