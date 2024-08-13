import queryString from 'query-string';
import React from 'react';

import { ILeaseFilter } from '@/features/leases';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_FileChecklistItem } from '@/models/api/generated/ApiGen_Concepts_FileChecklistItem';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseRenewal } from '@/models/api/generated/ApiGen_Concepts_LeaseRenewal';
import { ApiGen_Concepts_LeaseStakeholderType } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholderType';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

import { IPaginateRequest } from './interfaces/IPaginateRequest';
import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the lease endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiLeases = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getLeases: (params: IPaginateLeases | null) =>
        api.get<ApiGen_Base_Page<ApiGen_Concepts_Lease>>(
          `/leases/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getApiLease: (id: number) => api.get<ApiGen_Concepts_Lease>(`/leases/${id}`),
      getLastUpdatedByApi: (id: number) => api.get<Api_LastUpdatedBy>(`/leases/${id}/updateInfo`),
      postLease: (lease: ApiGen_Concepts_Lease, userOverrideCodes: UserOverrideCode[]) =>
        api.post<ApiGen_Concepts_Lease>(
          `/leases?${userOverrideCodes.map(o => `userOverrideCodes=${o}`).join('&')}`,
          lease,
        ),
      putApiLease: (lease: ApiGen_Concepts_Lease, userOverrideCodes: UserOverrideCode[] = []) =>
        api.put<ApiGen_Concepts_Lease>(
          `/leases/${lease.id}?${userOverrideCodes.map(o => `userOverrideCodes=${o}`).join('&')}`,
          lease,
        ),
      exportLeases: (filter: IPaginateLeases, outputFormat: 'csv' | 'excel' = 'excel') =>
        api.get<Blob>(
          `/reports/leases?${filter ? queryString.stringify({ ...filter, all: true }) : ''}`,
          {
            responseType: 'blob',
            headers: {
              Accept: outputFormat === 'csv' ? 'text/csv' : 'application/vnd.ms-excel',
            },
          },
        ),
      exportAggregatedLeases: (fiscalYearStart: number) =>
        api.get<Blob>(`/reports/leases/aggregated?fiscalYearStart=${fiscalYearStart}`, {
          responseType: 'blob',
          headers: {
            Accept: 'application/vnd.ms-excel',
          },
        }),
      exportLeasePayments: (fiscalYearStart: number) =>
        api.get<Blob>(`/reports/leases/payments?fiscalYearStart=${fiscalYearStart}`, {
          responseType: 'blob',
          headers: {
            Accept: 'application/vnd.ms-excel',
          },
        }),
      getLeaseRenewals: (leaseId: number) =>
        api.get<ApiGen_Concepts_LeaseRenewal[]>(`/leases/${leaseId}/renewals`),
      getLeaseChecklist: (leaseId: number) =>
        api.get<ApiGen_Concepts_FileChecklistItem[]>(`/leases/${leaseId}/checklist`),
      putLeaseChecklist: (lease: ApiGen_Concepts_FileWithChecklist) =>
        api.put<ApiGen_Concepts_Lease>(`/leases/${lease?.id}/checklist`, lease.fileChecklistItems),
      getLeaseStakeholderTypes: () =>
        api.get<ApiGen_Concepts_LeaseStakeholderType[]>(`/leases/stakeholdertypes`),
    }),
    [api],
  );
};

export type IPaginateLeases = IPaginateRequest<ILeaseFilter>;
