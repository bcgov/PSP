import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import {
  deletePropertyImprovementsApi,
  getPropertyImprovementApi,
  getPropertyImprovementsApi,
  postPropertyImprovementsApi,
  putPropertyImprovementsApi,
} from '../pims-api/useApiPropertyImprovements';
import { useApiRequestWrapper } from '../util/useApiRequestWrapper';

/**
 * hook that interacts with the property improvements API.
 */
export const usePropertyImprovementRepository = () => {
  const getPropertyImprovements = useApiRequestWrapper<
    (propertyId: number) => Promise<AxiosResponse<ApiGen_Concepts_PropertyImprovement[], any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number) => await getPropertyImprovementsApi(propertyId),
      [],
    ),
    requestName: 'getPropertyImprovements',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const postPropertyImprovement = useApiRequestWrapper<
    (
      propertyId: number,
      propertyImprovement: ApiGen_Concepts_PropertyImprovement,
    ) => Promise<AxiosResponse<ApiGen_Concepts_PropertyImprovement, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, improvement: ApiGen_Concepts_PropertyImprovement) =>
        await postPropertyImprovementsApi(propertyId, improvement),
      [],
    ),
    requestName: 'postPropertyImprovement',
    onError: useAxiosErrorHandler('Failed to create Property Improvement'),
  });

  const getPropertyImprovement = useApiRequestWrapper<
    (
      propertyId: number,
      propertyImprovementId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_PropertyImprovement, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, propertyImprovementId: number) =>
        await getPropertyImprovementApi(propertyId, propertyImprovementId),
      [],
    ),
    requestName: 'getPropertyImprovement',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to get Property Improvement by Id'),
  });

  const putPropertyImprovement = useApiRequestWrapper<
    (
      propertyId: number,
      propertyImprovementId: number,
      propertyImprovement: ApiGen_Concepts_PropertyImprovement,
    ) => Promise<AxiosResponse<ApiGen_Concepts_PropertyImprovement, any>>
  >({
    requestFunction: useCallback(
      async (
        propertyId: number,
        propertyImprovementId: number,
        improvement: ApiGen_Concepts_PropertyImprovement,
      ) => await putPropertyImprovementsApi(propertyId, propertyImprovementId, improvement),
      [],
    ),
    requestName: 'putPropertyImprovement',
    onError: useAxiosErrorHandler('Failed to update Property Improvement'),
  });

  const deletePropertyImprovement = useApiRequestWrapper<
    (propertyId: number, propertyImprovementId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, propertyImprovementId: number) =>
        await deletePropertyImprovementsApi(propertyId, propertyImprovementId),
      [],
    ),
    requestName: 'deletePropertyImprovement',
    onError: useAxiosErrorHandler('Failed to delete Property Improvement'),
  });

  return useMemo(
    () => ({
      getPropertyImprovements,
      postPropertyImprovement,
      getPropertyImprovement,
      putPropertyImprovement,
      deletePropertyImprovement,
    }),
    [
      deletePropertyImprovement,
      getPropertyImprovement,
      getPropertyImprovements,
      postPropertyImprovement,
      putPropertyImprovement,
    ],
  );
};
