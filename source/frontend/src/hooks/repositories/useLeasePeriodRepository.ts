import { AxiosResponse } from 'axios';
import { useCallback } from 'react';

import { ApiGen_Concepts_LeasePeriod } from '@/models/api/generated/ApiGen_Concepts_LeasePeriod';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import {
  deleteLeasePeriod,
  getLeasePeriods,
  postLeasePeriod,
  putLeasePeriod,
} from '../pims-api/useApiLeasePeriods';
import { useApiRequestWrapper } from '../util/useApiRequestWrapper';
import useDeepCompareMemo from '../util/useDeepCompareMemo';

/**
 * hook that interacts with the lease period API.
 */
export const useLeasePeriodRepository = () => {
  const getLeasePeriodsApi = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<ApiGen_Concepts_LeasePeriod[], any>>
  >({
    requestFunction: useCallback(async (leaseId: number) => await getLeasePeriods(leaseId), []),
    requestName: 'getLeasePeriods',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const updateLeasePeriodApi = useApiRequestWrapper<
    (
      period: ApiGen_Concepts_LeasePeriod,
    ) => Promise<AxiosResponse<ApiGen_Concepts_LeasePeriod, any>>
  >({
    requestFunction: useCallback(
      async (period: ApiGen_Concepts_LeasePeriod) => await putLeasePeriod(period),
      [],
    ),
    requestName: 'putLeasePeriod',
    onSuccess: useAxiosSuccessHandler('Period saved successfully.'),
    onError: useAxiosErrorHandler(),
  });

  const addLeasePeriodApi = useApiRequestWrapper<
    (
      period: ApiGen_Concepts_LeasePeriod,
    ) => Promise<AxiosResponse<ApiGen_Concepts_LeasePeriod, any>>
  >({
    requestFunction: useCallback(
      async (period: ApiGen_Concepts_LeasePeriod) => await postLeasePeriod(period),
      [],
    ),
    requestName: 'postLeasePeriod',
    onSuccess: useAxiosSuccessHandler('Period saved successfully.'),
    onError: useAxiosErrorHandler(),
  });

  const deleteLeasePeriodApi = useApiRequestWrapper<
    (period: ApiGen_Concepts_LeasePeriod) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (period: ApiGen_Concepts_LeasePeriod) => await deleteLeasePeriod(period),
      [],
    ),
    requestName: 'deleteLeasePeriod',
    onSuccess: useAxiosSuccessHandler('Period deleted successfully.'),
    onError: useAxiosErrorHandler(),
  });

  return useDeepCompareMemo(
    () => ({
      getLeasePeriods: getLeasePeriodsApi,
      updateLeasePeriod: updateLeasePeriodApi,
      addLeasePeriod: addLeasePeriodApi,
      deleteLeasePeriod: deleteLeasePeriodApi,
    }),
    [getLeasePeriodsApi, updateLeasePeriodApi, addLeasePeriodApi, deleteLeasePeriodApi],
  );
};
