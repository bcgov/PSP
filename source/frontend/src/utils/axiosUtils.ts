import axios, { AxiosError } from 'axios';
import { useCallback } from 'react';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import { IApiError } from '@/interfaces/IApiError';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

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
export function useAxiosErrorHandler(message = 'Network error. Check responses and try again.') {
  return useCallback(
    (axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error, { autoClose: 10000 });
        return Promise.resolve();
      } else {
        toast.error(message);
        return Promise.reject(axiosError);
      }
    },
    [message],
  );
}

/**
 * Provides default boilerplate applicable to handling most common axios request errors.
 * @param axiosError The request error object
 */
export function useAxiosErrorHandlerWithAuthorization(
  message = 'Network error. Check responses and try again.',
) {
  const history = useHistory();
  return useCallback(
    (axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error, { autoClose: 10000 });
        return Promise.resolve();
      } else if (axiosError?.response?.status === 403) {
        history.push('/forbidden');
        return Promise.resolve();
      } else {
        toast.error(message);
        return Promise.reject(axiosError);
      }
    },
    [history, message],
  );
}

export function useAxiosErrorHandlerWithConfirmation(
  needsUserAction: (
    userOverrideCode: UserOverrideCode | null,
    message: string | null,
    previousUserOverrideCodes: UserOverrideCode[],
  ) => void,
  message = 'Network error. Check responses and try again.',
) {
  return useCallback(
    (
      error: unknown,
      handleError?: (e: AxiosError<IApiError>) => void,
      previousUserOverrideCodes?: UserOverrideCode[],
    ) => {
      if (axios.isAxiosError(error)) {
        const axiosError = error as AxiosError<IApiError>;
        if (axiosError?.response?.status === 409) {
          // The API sent a 409 error - indicating user confirmation is needed OR not
          const userOverrideCode = Object.keys(UserOverrideCode).includes(
            axiosError?.response?.data?.errorCode,
          )
            ? (axiosError?.response?.data?.errorCode as UserOverrideCode)
            : null;

          if (userOverrideCode) {
            needsUserAction(
              userOverrideCode,
              axiosError?.response?.data?.error ?? null,
              previousUserOverrideCodes ?? [],
            );
          } else {
            toast.error(axiosError?.response.data.error);
          }
        } else if (axiosError?.response?.status === 400) {
          if (handleError) {
            handleError(axiosError);
          } else {
            toast.error(axiosError?.response.data.error);
          }
        } else {
          toast.error(message);
        }
      }
    },
    [message, needsUserAction],
  );
}
