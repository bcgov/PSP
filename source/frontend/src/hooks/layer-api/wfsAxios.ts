import axios, { AxiosError } from 'axios';
import { toast } from 'react-toastify';
import * as rax from 'retry-axios';

import { layerData } from '@/constants/toasts';
import { store } from '@/store/store';

const MAX_RETRIES = 2;

export const wfsAxios = (timeout?: number, onLayerError?: () => void) => {
  const instance = axios.create({ timeout: timeout ?? 5000 });
  instance.defaults.raxConfig = {
    retry: MAX_RETRIES,
    instance: instance,
    shouldRetry: (error: AxiosError) => {
      const cfg = rax.getConfig(error);
      if (cfg?.currentRetryAttempt === MAX_RETRIES) {
        !!onLayerError && onLayerError();
      }
      return rax.shouldRetryRequest(error);
    },
  };
  rax.attach(instance);

  return instance;
};

export const wfsAxios2 = (axiosProps?: { timeout?: number; authenticated?: boolean }) => {
  const instance = axios.create({
    timeout: axiosProps?.timeout ?? 5000,
  });
  if (axiosProps?.authenticated) {
    instance.defaults.headers.common['Authorization'] = `Bearer ${store.getState().jwt}`;
  }
  instance.defaults.raxConfig = {
    retry: MAX_RETRIES,
    instance: instance,
    shouldRetry: (error: AxiosError) => {
      const cfg = rax.getConfig(error);
      if (cfg?.currentRetryAttempt === MAX_RETRIES) {
        toast.dismiss(layerData.LAYER_DATA_LOADING_ID);
        layerData.LAYER_DATA_ERROR();
      }
      return rax.shouldRetryRequest(error);
    },
  };
  rax.attach(instance);

  instance.interceptors.request.use(config => {
    layerData.LAYER_DATA_LOADING();
    return config;
  });
  return instance;
};
