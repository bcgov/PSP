import { Api_Insurance } from 'models/api/Insurance';

import { IInsurance } from '@/interfaces';
import { IBatchUpdateReply, IBatchUpdateRequest } from '@/interfaces/batchUpdate';

import useAxiosApi from './useApi';
  );

export const getLeaseInsurances = (leaseId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<[]>(`/leases/${leaseId}/insurances`);
