import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { ApiGen_Concepts_Insurance } from '@/models/api/generated/ApiGen_Concepts_Insurance';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { getLeaseInsurances, updateLeaseInsurances } from '../pims-api/useApiInsurances';
import { useApiRequestWrapper } from '../util/useApiRequestWrapper';

/**
 * hook that interacts with the insurances API.
 */
export const useInsurancesRepository = () => {
  const getInsurancesApi = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<ApiGen_Concepts_Insurance[], any>>
  >({
    requestFunction: useCallback(async (leaseId: number) => await getLeaseInsurances(leaseId), []),
    requestName: 'getLeaseInsurances',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const updateInsurancesApi = useApiRequestWrapper<
    (
      leaseId: number,
      insurances: ApiGen_Concepts_Insurance[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_Insurance[], any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, insurances: ApiGen_Concepts_Insurance[]) =>
        await updateLeaseInsurances(leaseId, insurances),
      [],
    ),
    requestName: 'updateLeaseInsurances',
    onSuccess: useAxiosSuccessHandler('Insurance saved successfully.'),
    onError: useAxiosErrorHandler(),
  });

  return useMemo(
    () => ({
      getInsurances: getInsurancesApi,
      updateInsurances: updateInsurancesApi,
    }),
    [getInsurancesApi, updateInsurancesApi],
  );
};
