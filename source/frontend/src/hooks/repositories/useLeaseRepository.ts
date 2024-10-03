import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_FileChecklistItem } from '@/models/api/generated/ApiGen_Concepts_FileChecklistItem';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseRenewal } from '@/models/api/generated/ApiGen_Concepts_LeaseRenewal';
import { ApiGen_Concepts_LeaseStakeholderType } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholderType';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { useApiLeases } from '../pims-api/useApiLeases';

/**
 * hook that interacts with the Lease API.
 */
export const useLeaseRepository = () => {
  const {
    getLastUpdatedByApi,
    getApiLease,
    putApiLease,
    putLeaseChecklist,
    getLeaseChecklist,
    getLeaseRenewals,
    getLeaseStakeholderTypes,
  } = useApiLeases();

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

  const updateApiLease = useApiRequestWrapper<
    (
      lease: ApiGen_Concepts_Lease,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_Lease, any>>
  >({
    requestFunction: useCallback(
      async (lease: ApiGen_Concepts_Lease, userOverrideCodes: UserOverrideCode[] = []) =>
        await putApiLease(lease, userOverrideCodes),
      [putApiLease],
    ),
    requestName: 'updateLease',
    throwError: true,
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
    onError: useAxiosErrorHandler('Failed to retrieve lease.'),
  });

  const getLeaseRenewalsApi = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<ApiGen_Concepts_LeaseRenewal[], any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number) => await getLeaseRenewals(leaseId),
      [getLeaseRenewals],
    ),
    requestName: 'getLeaseRenewalsApi',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retrieve lease renewals.'),
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

  const getLeaseStakeholderTypesApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<ApiGen_Concepts_LeaseStakeholderType[], any>>
  >({
    requestFunction: useCallback(
      async () => await getLeaseStakeholderTypes(),
      [getLeaseStakeholderTypes],
    ),
    requestName: 'getLeaseStakeholderTypes',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to retrieve Lease Stakeholder Types.'),
  });

  return useMemo(
    () => ({
      getLastUpdatedBy: getLastUpdatedBy,
      updateLease: updateApiLease,
      getLease: getLease,
      getLeaseRenewals: getLeaseRenewalsApi,
      getLeaseChecklist: getLeaseChecklistApi,
      putLeaseChecklist: updateLeaseChecklistApi,
      getLeaseStakeholderTypes: getLeaseStakeholderTypesApi,
    }),
    [
      getLastUpdatedBy,
      updateApiLease,
      getLease,
      getLeaseRenewalsApi,
      getLeaseChecklistApi,
      updateLeaseChecklistApi,
      getLeaseStakeholderTypesApi,
    ],
  );
};
