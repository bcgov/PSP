import { ENVIRONMENT } from '@/constants';
import CustomAxios from '@/customAxios';
import { Api_PropertyLease } from '@/models/api/PropertyLease';

export const getPropertyLeases = (leaseId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_PropertyLease[]>(
    `/leases/${leaseId}/properties`,
  );
