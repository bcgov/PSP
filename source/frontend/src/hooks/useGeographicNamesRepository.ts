import { AxiosError, AxiosResponse } from 'axios';
import { FeatureCollection, Geometry } from 'geojson';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { IApiError } from '@/interfaces/IApiError';

import { IGeographicNamesProperties } from './pims-api/interfaces/IGeographicNamesProperties';
import {
  GeographicNameSearchCriteria,
  useApiGeographicNames,
} from './pims-api/useApiGeographicNames';
import { useApiRequestWrapper } from './util/useApiRequestWrapper';

export const useGeographicNamesRepository = () => {
  const { searchName } = useApiGeographicNames();

  // Search by address string
  const searchNameApi = useApiRequestWrapper<
    (
      searchCriteria: GeographicNameSearchCriteria,
    ) => Promise<AxiosResponse<FeatureCollection<Geometry, IGeographicNamesProperties>>>
  >({
    requestFunction: useCallback(
      async (searchCriteria: GeographicNameSearchCriteria) => await searchName(searchCriteria),
      [searchName],
    ),
    requestName: 'searchName',
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      toast.error(
        `Failed to geographic name information. Is the service down?: ${axiosError?.response?.data.error}`,
      );
      return Promise.resolve();
    }, []),
  });

  return {
    searchName: searchNameApi,
  };
};
