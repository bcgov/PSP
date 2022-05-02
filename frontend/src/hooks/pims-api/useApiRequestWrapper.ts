import axios, { AxiosError, AxiosResponse } from 'axios';
import useIsMounted from 'hooks/useIsMounted';
import { IApiError } from 'interfaces/IApiError';
import { useCallback, useEffect, useMemo, useState } from 'react';
import { useDispatch } from 'react-redux';
import { handleAxiosResponse } from 'utils';

export interface IResponseWrapper<
  FunctionType extends (...args: any[]) => Promise<AxiosResponse<unknown | undefined>>
> {
  error: AxiosError<IApiError, any> | undefined;
  response: Awaited<ReturnType<FunctionType>>['data'] | undefined;
  execute: (
    ...params: Parameters<FunctionType> | []
  ) => Promise<Awaited<ReturnType<FunctionType>>['data'] | undefined>;
  loading?: boolean;
}

export interface IApiRequestWrapper<
  FunctionType extends (...args: any[]) => Promise<AxiosResponse<unknown | undefined>>
> {
  requestFunction: FunctionType;
  requestName: string;
  onSuccess?: (response: Awaited<ReturnType<FunctionType>>['data'] | undefined) => void;
  onError?: (e: AxiosError<IApiError>) => void;
  invoke?: boolean;
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
  FunctionType extends (...args: any[]) => Promise<AxiosResponse<unknown | undefined>>
>({
  requestFunction,
  requestName,
  onSuccess,
  onError,
  invoke,
}: IApiRequestWrapper<FunctionType>): IResponseWrapper<FunctionType> => {
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<AxiosError<IApiError>>();
  const [response, setResponse] = useState<Awaited<ReturnType<FunctionType>>['data'] | undefined>();
  const dispatch = useDispatch();
  const isMounted = useIsMounted();

  // allow direct access to the api wrapper function in case direct invocation is required by consumer.
  const wrappedApiRequest = useCallback<
    (...params: Parameters<FunctionType> | []) => Promise<unknown | undefined>
  >(
    async (...args) => {
      try {
        // reset initial state whenever a request is started.
        setLoading(true);
        setError(undefined);
        setResponse(undefined);
        const response = await handleAxiosResponse<
          Awaited<ReturnType<FunctionType>>['data'] | undefined
        >(dispatch, requestName, requestFunction(...args));
        if (!isMounted()) {
          return;
        }
        setResponse(response);
        onSuccess && onSuccess(response);
        return response;
      } catch (e) {
        if (!axios.isAxiosError(e)) {
          throw e;
        }
        if (!isMounted()) {
          return;
        }
        const axiosError = e as AxiosError<IApiError>;
        onError && onError(axiosError);
        setError(axiosError);
      } finally {
        if (!isMounted()) {
          return;
        }
        setLoading(false);
      }
    },
    [dispatch, isMounted, onError, onSuccess, requestFunction, requestName],
  );

  useEffect(() => {
    if (invoke) {
      wrappedApiRequest();
    }
  }, [wrappedApiRequest, invoke]);

  return useMemo(
    () => ({
      error: error,
      response: response,
      loading: loading,
      execute: wrappedApiRequest,
    }),
    [error, loading, response, wrappedApiRequest],
  );
};
