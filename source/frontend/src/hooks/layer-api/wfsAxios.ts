import axios, { AxiosError } from 'axios';
import * as rax from 'retry-axios';

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
