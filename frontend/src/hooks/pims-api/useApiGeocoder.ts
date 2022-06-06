import { IResearchFilter } from 'features/research/interfaces';
import { IGeocoderPidsResponse, IGeocoderResponse } from 'hooks/useApi';
import React from 'react';

import { IPaginateRequest, useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the geocoder endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiGeocoder = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      searchAddress: (address: string, additionalQS?: string) =>
        api.get<IGeocoderResponse[]>(
          `/tools/geocoder/addresses?address=${address}${additionalQS ? `&${additionalQS}` : ``}`,
        ),
      getSitePids: (siteId: string) =>
        api.get<IGeocoderPidsResponse>(`/tools/geocoder/parcels/pids/${siteId}`),
    }),
    [api],
  );
};

export type IPaginateResearch = IPaginateRequest<IResearchFilter>;

export const mockGeocoderPidsResponse: IGeocoderPidsResponse = {
  pids: ['9025ea39-da19-4655-85e6-e34c00e765f8'],
  siteId: '312312',
};

export const mockGeocoderOptions: IGeocoderResponse[] = [
  {
    fullAddress: '1234 Fake St',
    siteId: '1',
    address1: '1234 Fake St',
    administrativeArea: 'Test Town',
    provinceCode: 'BC',
    latitude: 1,
    longitude: 1,
    score: 60,
  },
  {
    fullAddress: '5521 Test St',
    siteId: '1',
    address1: '5521 Test St',
    administrativeArea: 'Test Town',
    provinceCode: 'BC',
    latitude: 2,
    longitude: 2,
    score: 70,
  },
];
