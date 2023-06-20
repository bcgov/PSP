import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { Api_PropertyImprovement } from '@/models/api/PropertyImprovement';
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
    (leaseId: number) => Promise<AxiosResponse<Api_PropertyImprovement[], any>>
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
      improvements: Api_PropertyImprovement[],
    ) => Promise<AxiosResponse<Api_PropertyImprovement[], any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, improvements: Api_PropertyImprovement[]) =>
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
