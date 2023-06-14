import React from 'react';

import { IResearchFilter } from '@/features/research/interfaces';

import { IGeocoderPidsResponse, IGeocoderResponse } from './interfaces/IGeocoder';
import { IPaginateRequest } from './interfaces/IPaginateRequest';
import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the geocoder endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiGeocoder = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      searchAddressApi: (address: string, additionalQS?: string) =>
        api.get<IGeocoderResponse[]>(
          `/tools/geocoder/addresses?address=${address}${additionalQS ? `&${additionalQS}` : ``}`,
        ),
      getSitePidsApi: (siteId: string) =>
        api.get<IGeocoderPidsResponse>(`/tools/geocoder/parcels/pids/${siteId}`),
      getNearestToPointApi: (lng: number, lat: number) =>
        api.get<IGeocoderResponse>(`/tools/geocoder/nearest?point=${lng},${lat}`),
    }),
    [api],
  );
};

export type IPaginateResearch = IPaginateRequest<IResearchFilter>;
