import { ENVIRONMENT } from 'constants/environment';
import CustomAxios from 'customAxios';
import { LatLngLiteral } from 'leaflet';
import _ from 'lodash';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { store } from 'store/store';

export interface IGeocoderResponse {
  siteId: string;
  fullAddress: string;
  address1: string;
  administrativeArea: string;
  provinceCode: string;
  latitude: number;
  longitude: number;
  score: number;
}

export interface IGeocoderPidsResponse {
  siteId: string;
  pids: string[];
}

export interface IPimsAPI {
  searchAddress: (text: string) => Promise<IGeocoderResponse[]>;
  getSitePids: (siteId: string) => Promise<IGeocoderPidsResponse>;
  getNearAddresses: (latLng: LatLngLiteral) => Promise<IGeocoderResponse[]>;
  getNearestAddress: (latLng: LatLngLiteral) => Promise<IGeocoderResponse>;
}

/**
 * TODO: PSP-4408 This hook needs to get deleted.
 * @deprecated The /hooks/pims-api hooks should be used instead.
 */
export const useApi = (): IPimsAPI => {
  const dispatch = useDispatch();

  const getAxios = useCallback(() => {
    const axios = CustomAxios();
    axios.interceptors.request.use(
      config => {
        if (config.headers === undefined) {
          config.headers = {};
        }
        config.headers.Authorization = `Bearer ${store.getState().jwt}`;
        dispatch(showLoading());
        return config;
      },
      error => {
        dispatch(hideLoading());
        return Promise.reject(error);
      },
    );

    axios.interceptors.response.use(
      config => {
        dispatch(hideLoading());
        return config;
      },
      error => {
        dispatch(hideLoading());
        return Promise.reject(error);
      },
    );
    return axios;
  }, [dispatch]);

  const searchAddress = useCallback(
    async (address: string): Promise<IGeocoderResponse[]> => {
      const { data } = await getAxios().get<IGeocoderResponse[]>(
        `${ENVIRONMENT.apiUrl}/tools/geocoder/addresses?address=${address}+BC`,
      );
      return _.orderBy(data, (r: IGeocoderResponse) => r.score, ['desc']);
    },
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [],
  );

  const getSitePids = useCallback(
    async (siteId: string): Promise<IGeocoderPidsResponse> => {
      const { data } = await getAxios().get<IGeocoderPidsResponse>(
        `${ENVIRONMENT.apiUrl}/tools/geocoder/parcels/pids/${siteId}`,
      );
      return data;
    },
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [],
  );

  /**
   * Get the nearest geocoded address to the given lat/lng point.
   */
  const getNearestAddress = useCallback(
    async (latLng: LatLngLiteral): Promise<IGeocoderResponse> => {
      const { data } = await getAxios().get<IGeocoderResponse>(
        `${ENVIRONMENT.apiUrl}/tools/geocoder/nearest?point=${latLng.lng},${latLng.lat}`,
      );
      return data;
    },
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [],
  );

  /**
   * Get the 5 nearest geocoded addresses to the given lat/lng point.
   */
  const getNearAddresses = useCallback(
    async (latLng: LatLngLiteral): Promise<IGeocoderResponse[]> => {
      const { data } = await getAxios().get<IGeocoderResponse[]>(
        `${ENVIRONMENT.apiUrl}/tools/geocoder/near?point=${latLng.lng},${latLng.lat}`,
      );
      return data;
    },
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [],
  );

  // The below memo is only intended to run once, at startup. Or when the jwt is updated.
  // eslint-disable-next-line react-hooks/exhaustive-deps
  return {
    getSitePids,
    searchAddress,
    getNearestAddress,
    getNearAddresses,
  };
};
