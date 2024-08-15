import { ENVIRONMENT } from '@/constants';
import CustomAxios from '@/customAxios';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';

export const updateLeaseStakeholders = (
  leaseId: number,
  improvements: ApiGen_Concepts_LeaseStakeholder[],
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_LeaseStakeholder[]>(
    `/leases/${leaseId}/stakeholders`,
    improvements,
  );

export const getLeaseStakeholders = (leaseId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_LeaseStakeholder[]>(
    `/leases/${leaseId}/stakeholders`,
  );
