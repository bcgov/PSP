import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_FileChecklistItem } from '@/models/api/generated/ApiGen_Concepts_FileChecklistItem';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { useApiLeases } from '../pims-api/useApiLeases';

/**
 * hook that interacts with the Lease API.
 */
export const useLeaseRepository = () => {
  const { getLastUpdatedByApi, getApiLease, putLeaseChecklist, getLeaseChecklist } = useApiLeases();

  const getLastUpdatedBy = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<Api_LastUpdatedBy, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number) => await getLastUpdatedByApi(leaseId),
      [getLastUpdatedByApi],
    ),
    requestName: 'getLastUpdatedBy',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retreive last-updated-by information for a lease.'),
  });

  const getLease = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<ApiGen_Concepts_Lease, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number) => await getApiLease(leaseId),
      [getApiLease],
    ),
    requestName: 'getApiLease',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retreive lease.'),
  });

  const getLeaseChecklistApi = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<ApiGen_Concepts_FileChecklistItem[], any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number) => await getLeaseChecklist(leaseId),
      [getLeaseChecklist],
    ),
    requestName: 'getApiLeaseChecklist',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retreive lease checklist.'),
  });

  const updateLeaseChecklistApi = useApiRequestWrapper<
    (lease: ApiGen_Concepts_FileWithChecklist) => Promise<AxiosResponse<ApiGen_Concepts_Lease, any>>
  >({
    requestFunction: useCallback(
      async (lease: ApiGen_Concepts_FileWithChecklist) => await putLeaseChecklist(lease),
      [putLeaseChecklist],
    ),
    requestName: 'UpdateLeaseChecklist',
    onError: useAxiosErrorHandler('Failed to update Lease Checklist'),
    throwError: true,
  });

  return useMemo(
    () => ({
      getLastUpdatedBy: getLastUpdatedBy,
      getLease: getLease,
      getLeaseChecklist: getLeaseChecklistApi,
      putLeaseChecklist: updateLeaseChecklistApi,
    }),
    [getLastUpdatedBy, getLease, getLeaseChecklistApi, updateLeaseChecklistApi],
  );
};
