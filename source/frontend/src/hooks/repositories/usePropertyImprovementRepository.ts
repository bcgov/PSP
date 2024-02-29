import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import {
  getPropertyImprovements,
  updatePropertyImprovements,
} from '../pims-api/useApiPropertyImprovements';
import { useApiRequestWrapper } from '../util/useApiRequestWrapper';

/**
 * hook that interacts with the property improvements API.
 */
export const usePropertyImprovementRepository = () => {
  const getPropertyImprovementsApi = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<ApiGen_Concepts_PropertyImprovement[], any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number) => await getPropertyImprovements(leaseId),
      [],
    ),
    requestName: 'getPropertyImprovements',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const updatePropertyImprovementsApi = useApiRequestWrapper<
    (
      leaseId: number,
      improvements: ApiGen_Concepts_PropertyImprovement[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_PropertyImprovement[], any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, improvements: ApiGen_Concepts_PropertyImprovement[]) =>
        await updatePropertyImprovements(leaseId, improvements),
      [],
    ),
    requestName: 'updatePropertyImprovements',
    onSuccess: useAxiosSuccessHandler('Improvements saved successfully.'),
    onError: useAxiosErrorHandler(),
  });

  return useMemo(
    () => ({
      getPropertyImprovements: getPropertyImprovementsApi,
      updatePropertyImprovements: updatePropertyImprovementsApi,
    }),
    [getPropertyImprovementsApi, updatePropertyImprovementsApi],
  );
};
