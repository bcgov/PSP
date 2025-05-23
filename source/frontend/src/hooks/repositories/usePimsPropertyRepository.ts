import { AxiosError, AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';
import { toast } from 'react-toastify';

import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { Api_PropertyFilterCriteria } from '@/models/api/ProjectFilterCriteria';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

/**
 * hook that retrieves a property from the inventory.
 */
export const usePimsPropertyRepository = () => {
  const {
    getPropertyConceptWithIdApi,
    putPropertyConceptApi,
    getMatchingPropertiesApi,
    getPropertyConceptWithPidApi,
    getPropertyConceptWithPinApi,
    getPropertiesApi,
  } = useApiProperties();

  const getPropertyWrapper = useApiRequestWrapper({
    requestFunction: useCallback(
      async (id: number) => await getPropertyConceptWithIdApi(id),
      [getPropertyConceptWithIdApi],
    ),
    requestName: 'getPropertyApiById',
    onError: useAxiosErrorHandler('Failed to retrieve property information from PIMS'),
  });

  const getPropertyByPidWrapper = useApiRequestWrapper<
    (...args: any[]) => Promise<AxiosResponse<ApiGen_Concepts_Property, any>>
  >({
    requestFunction: useCallback(
      async (pid: string) => await getPropertyConceptWithPidApi(pid),
      [getPropertyConceptWithPidApi],
    ),
    requestName: 'getPropertyConceptWithPidApi',
    throwError: true,
  });

  const getPropertyByPinWrapper = useApiRequestWrapper<
    (...args: any[]) => Promise<AxiosResponse<ApiGen_Concepts_Property, any>>
  >({
    requestFunction: useCallback(
      async (pin: number) => await getPropertyConceptWithPinApi(pin),
      [getPropertyConceptWithPinApi],
    ),
    requestName: 'getPropertyConceptWithPinApi',
    throwError: true,
  });

  const getMatchingProperties = useApiRequestWrapper({
    requestFunction: useCallback(
      async (filterCriteria: Api_PropertyFilterCriteria) =>
        await getMatchingPropertiesApi(filterCriteria),
      [getMatchingPropertiesApi],
    ),
    requestName: 'getMatchingProperties',
    onError: useAxiosErrorHandler(
      'Failed to retrieve property information from PIMS matching filter criteria',
    ),
  });

  const getAllPropertiesById = useApiRequestWrapper({
    requestFunction: useCallback(
      async (propertyIds: number[]) => await getPropertiesApi(propertyIds),
      [getPropertiesApi],
    ),
    requestName: 'getAllPropertiesById',
    onError: useAxiosErrorHandler(
      'Failed to retrieve property information from PIMS matching filter criteria',
    ),
  });

  const updatePropertyWrapper = useApiRequestWrapper({
    requestFunction: useCallback(
      async (property: ApiGen_Concepts_Property) => await putPropertyConceptApi(property),
      [putPropertyConceptApi],
    ),
    requestName: 'updatePropertyConcept',
    onSuccess: useAxiosSuccessHandler(),
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
        return Promise.resolve();
      } else {
        toast.error('Save error. Check responses and try again.');
        return Promise.reject(axiosError);
      }
    }, []),
  });

  return useMemo(
    () => ({
      getPropertyWrapper,
      updatePropertyWrapper,
      getMatchingProperties,
      getPropertyByPidWrapper,
      getPropertyByPinWrapper,
      getAllPropertiesById,
    }),
    [
      getPropertyWrapper,
      updatePropertyWrapper,
      getMatchingProperties,
      getPropertyByPidWrapper,
      getPropertyByPinWrapper,
      getAllPropertiesById,
    ],
  );
};
