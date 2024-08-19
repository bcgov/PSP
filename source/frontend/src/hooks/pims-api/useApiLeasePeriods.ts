import { ENVIRONMENT } from '@/constants';
import CustomAxios from '@/customAxios';
import { ApiGen_Concepts_LeasePeriod } from '@/models/api/generated/ApiGen_Concepts_LeasePeriod';

export const getLeasePeriods = (leaseId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_LeasePeriod[]>(
    `/leases/${leaseId}/periods`,
  );
export const deleteLeasePeriod = (period: ApiGen_Concepts_LeasePeriod) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(
    `/leases/${period.leaseId}/periods`,
    {
      data: period,
    },
  );
export const putLeasePeriod = (period: ApiGen_Concepts_LeasePeriod) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_LeasePeriod>(
    `/leases/${period.leaseId}/periods/${period.id}`,
    period,
  );
export const postLeasePeriod = (period: ApiGen_Concepts_LeasePeriod) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<ApiGen_Concepts_LeasePeriod>(
    `/leases/${period.leaseId}/periods`,
    period,
  );
