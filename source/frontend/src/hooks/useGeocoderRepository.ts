import { AxiosError, AxiosResponse } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { IApiError } from '@/interfaces/IApiError';

import { IGeocoderPidsResponse, IGeocoderResponse } from './pims-api/interfaces/IGeocoder';
import { useApiGeocoder } from './pims-api/useApiGeocoder';
import { useApiRequestWrapper } from './util/useApiRequestWrapper';

export const useGeocoderRepository = () => {
  const { getSitePidsApi, searchAddressApi, getNearestToPointApi } = useApiGeocoder();

  // Retrieve by site id
  const { execute: executeSitePids, loading: isLoadingSitePids } = useApiRequestWrapper<
    (siteId: string) => Promise<AxiosResponse<IGeocoderPidsResponse>>
  >({
    requestFunction: useCallback(
      async (siteId: string) => await getSitePidsApi(siteId),
      [getSitePidsApi],
    ),
    requestName: 'getSitePids',
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      toast.error(
        `Failed to get PIMS property data. error from backend: ${axiosError?.response?.data.error}`,
      );
    }, []),
  });

  // Search by address string
  const { execute: executeSearchAddress, loading: isLoadingSearchAddress } = useApiRequestWrapper<
    (address: string, additionalQS?: string) => Promise<AxiosResponse<IGeocoderResponse[]>>
  >({
    requestFunction: useCallback(
      async (address: string, additionalQS?: string) =>
        await searchAddressApi(address, additionalQS),
      [searchAddressApi],
    ),
    requestName: 'searchAddress',
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      toast.error(
        `Failed to get PIMS property data. error from backend: ${axiosError?.response?.data.error}`,
      );
    }, []),
  });

  // Get nearest to point (longitude, latitude)
  const { execute: executeNearestToPoint, loading: isLoadingNearestToPoint } = useApiRequestWrapper<
    (lng: number, lat: number) => Promise<AxiosResponse<IGeocoderResponse>>
  >({
    requestFunction: useCallback(
      async (lng: number, lat: number) => await getNearestToPointApi(lng, lat),
      [getNearestToPointApi],
    ),
    requestName: 'getNearestToPoint',
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      toast.error(
        `Failed to get PIMS property data. error from backend: ${axiosError?.response?.data.error}`,
      );
    }, []),
  });

  return {
    getSitePids: executeSitePids,
    isLoadingSitePids,
    searchAddress: executeSearchAddress,
    isLoadingSearchAddress,
    getNearestToPoint: executeNearestToPoint,
    isLoadingNearestToPoint,
  };
};
