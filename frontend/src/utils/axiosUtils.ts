import { AxiosError } from 'axios';
import { IApiError } from 'interfaces/IApiError';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

/**
 * Provides default boilerplate applicable to handling axios requests completed successfully.
 * @param message The message to show as a toast
 */
export function useAxiosSuccessHandler(message?: string) {
  return useCallback(() => {
    if (message) {
      toast.success(message);
    }
  }, [message]);
}

/**
 * Provides default boilerplate applicable to handling most common axios request errors.
 * @param axiosError The request error object
 */
export function useAxiosErrorHandler(message = 'Save error. Check responses and try again.') {
  return useCallback(
    (axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error(message);
      }
    },
    [message],
  );
}
