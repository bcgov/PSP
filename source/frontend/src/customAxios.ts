import { Dispatch } from '@reduxjs/toolkit';
import axios, { AxiosError, AxiosRequestHeaders } from 'axios';
import isEmpty from 'lodash/isEmpty';
import { hideLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';

import { IGenericNetworkAction } from '@/store/slices/network/interfaces';
import { logError } from '@/store/slices/network/networkSlice';
import { RootState, store } from '@/store/store';

export const defaultEnvelope = (x: any) => ({ data: { records: x } });

/**
 * used by the CustomAxios method.
 * loadingToast is the message to display while the api request is pending. This toast is cancelled when the request is completed.
 * successToast is displayed when the request is completed successfully.
 * errorToast is displayed when the request fails for any reason. By default this will return an error from axios.
 */
export interface LifecycleToasts {
  loadingToast?: () => React.ReactText;
  successToast?: () => React.ReactText;
  errorToast?: () => React.ReactText;
}

/**
 * Wrapper for axios to include authentication token and error handling.
 * @param param0 Axios parameters.
 */
export const CustomAxios = ({
  lifecycleToasts,
  selector,
  envelope = defaultEnvelope,
  baseURL,
}: {
  lifecycleToasts?: LifecycleToasts;
  selector?: (state: RootState) => RootState;
  envelope?: typeof defaultEnvelope;
  baseURL?: string;
} = {}) => {
  baseURL = baseURL ?? '/';
  let loadingToastId: React.ReactText | undefined = undefined;
  const instance = axios.create({
    baseURL,
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Cache-Control': import.meta.env.DEV ? 'no-cache' : '',
    },
  });
  instance.interceptors.request.use(config => {
    if (config.headers === undefined) {
      config.headers = {} as AxiosRequestHeaders;
    }
    config.headers.Authorization = `Bearer ${store.getState().jwt}`;
    if (selector !== undefined) {
      const state = store.getState();
      const storedValue = selector(state);

      if (!isEmpty(storedValue)) {
        throw new axios.Cancel(JSON.stringify(envelope(storedValue)));
      }
    }
    if (lifecycleToasts?.loadingToast) {
      loadingToastId = lifecycleToasts.loadingToast();
    }
    return config;
  });

  instance.interceptors.response.use(
    response => {
      if (lifecycleToasts?.successToast && response.status < 300) {
        loadingToastId && toast.dismiss(loadingToastId);
        lifecycleToasts.successToast();
      } else if (lifecycleToasts?.errorToast && response.status >= 300) {
        lifecycleToasts.errorToast();
      }
      return response;
    },
    error => {
      if (axios.isCancel(error)) {
        return Promise.resolve(error.message);
      }
      if (lifecycleToasts?.errorToast) {
        loadingToastId && toast.dismiss(loadingToastId);
        lifecycleToasts.errorToast();
      }

      return Promise.reject(error);
    },
  );

  return instance;
};

export const catchAxiosError = (
  axiosError: AxiosError,
  dispatch: Dispatch<any>,
  errorNetworkAction: string,
) => {
  const payload: IGenericNetworkAction = {
    name: errorNetworkAction,
    status: axiosError?.response?.status,
    error: axiosError,
  };
  dispatch(logError(payload));
  dispatch(hideLoading());
  throw Error(axiosError.message);
};

export default CustomAxios;
