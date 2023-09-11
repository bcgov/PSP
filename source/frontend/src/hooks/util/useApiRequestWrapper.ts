import axios, { AxiosError, AxiosResponse } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';

import useIsMounted from '@/hooks/util/useIsMounted';
import { IApiError } from '@/interfaces/IApiError';
import { handleAxiosResponse } from '@/utils';

export interface IResponseWrapper<
  FunctionType extends (...args: any) => Promise<AxiosResponse<unknown | undefined>>,
> {
  error: AxiosError<IApiError, any> | undefined;
  response: Awaited<ReturnType<FunctionType>>['data'] | undefined;
  execute: (
    ...params: Parameters<FunctionType>
  ) => Promise<Awaited<ReturnType<FunctionType>>['data'] | undefined>;
  loading: boolean;
  requestedOn?: Date;
  requestEndOn?: Date;
  status: Awaited<ReturnType<FunctionType>>['status'] | undefined;
}

export interface IApiRequestWrapper<
  FunctionType extends (...args: any) => Promise<AxiosResponse<unknown | undefined>>,
> {
  requestFunction: FunctionType;
  requestName: string;
  onSuccess?: (response: Awaited<ReturnType<FunctionType>>['data'] | undefined) => void;
  onError?: (e: AxiosError<IApiError>) => void;
  invoke?: boolean;
  skipErrorLogCodes?: number[];
  throwError?: boolean;
  rawResponse?: boolean;
}

type Awaited<T> = T extends PromiseLike<infer U> ? Awaited<U> : T;

/**
 * Wrapper for api requests that tracks the state of the response. Also returns the wrapped api request directly.
 * @param {(...params: Parameters<FunctionType>) => AxiosPromise<ResponseType>} requestFunction Any promise that returns an axios response an success and an axios error on failure.
 * @param {string} requestName A unique name used to identify this wrapped request.
 * @param {(response: Awaited<ReturnType<FunctionType>>) => void} onSuccess A function to execute when the request completes successfully.
 * @param {(e: AxiosError) => void} onError A function to execute when the request throws an error.
 * @param {boolean} invoke immediately invoke the wrapped function.
 */
export const useApiRequestWrapper = <
  FunctionType extends (...args: any) => Promise<AxiosResponse<unknown | undefined>>,
>({
  requestFunction,
  requestName,
  onSuccess,
  onError,
  invoke,
  skipErrorLogCodes,
  throwError,
  rawResponse,
}: IApiRequestWrapper<FunctionType>): IResponseWrapper<FunctionType> => {
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<AxiosError<IApiError>>();
  const [requestedOn, setRequestedOn] = useState<Date>();
  const [requestEndOn, setRequestEndOn] = useState<Date>();
  const [response, setResponse] = useState<Awaited<ReturnType<FunctionType>>['data'] | undefined>();
  const [status, setStatus] = useState<number>();
  const dispatch = useDispatch();
  const isMounted = useIsMounted();

  // allow direct access to the api wrapper function in case direct invocation is required by consumer.
  const wrappedApiRequest = useCallback<
    (params: Parameters<FunctionType>) => Promise<unknown | undefined>
  >(
    async (...args) => {
      try {
        // reset initial state whenever a request is started.
        setLoading(true);
        setRequestedOn(new Date());
        setError(undefined);
        setResponse(undefined);
        const response = !rawResponse
          ? await handleAxiosResponse<Awaited<ReturnType<FunctionType>>['data'] | undefined>(
              dispatch,
              requestName,
              requestFunction(...args),
              skipErrorLogCodes,
            )
          : await requestFunction(...args);
        if (!isMounted()) {
          return;
        }
        setStatus(response?.status);
        setResponse(response?.data);
        onSuccess && onSuccess(response?.data);
        return response?.data;
      } catch (e) {
        if (!axios.isAxiosError(e) && throwError) {
          throw e;
        }
        if (!isMounted()) {
          return;
        }
        const axiosError = e as AxiosError<IApiError>;
        onError && onError(axiosError);
        setError(axiosError);
        if (throwError) {
          throw e;
        }
      } finally {
        if (isMounted()) {
          setRequestEndOn(new Date());
          setLoading(false);
        }
      }
    },
    [
      rawResponse,
      dispatch,
      requestName,
      requestFunction,
      skipErrorLogCodes,
      isMounted,
      onSuccess,
      throwError,
      onError,
    ],
  );

  useEffect(() => {
    if (invoke) {
      // If invoke is set then the function is parameterless.
      wrappedApiRequest(undefined as Parameters<FunctionType>);
    }
  }, [wrappedApiRequest, invoke]);

  return {
    error: error,
    status: status,
    response: response,
    loading: loading,
    execute: wrappedApiRequest,
    requestedOn,
    requestEndOn,
  };
};
