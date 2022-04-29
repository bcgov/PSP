import axios, { AxiosError, AxiosResponse } from 'axios';
import useIsMounted from 'hooks/useIsMounted';
import { IApiError } from 'interfaces/IApiError';
import { useCallback, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { handleAxiosResponse } from 'utils';

export interface IResponseWrapper<ResponseType> {
  error: AxiosError;
  response: ResponseType;
  refresh: (...args: any[]) => Promise<ResponseType | undefined>;
  loading?: boolean;
}

export interface IApiRequestWrapper<ResponseType> {
  requestFunction: (...args: any[]) => Promise<AxiosResponse<ResponseType, any>>;
  requestName: string;
  onSuccess?: (response: ResponseType) => void;
  onError?: (e: AxiosError<IApiError>) => void;
  invoke?: boolean;
}

/**
 * Wrapper for api requests that tracks the state of the response. Also returns the wrapped api request directly.
 * @param {() => AxiosPromise<ResponseType>} requestFunction Any promise that returns an axios response an success and an axios error on failure.
 * @param {string} requestName A unique name used to identify this wrapped request.
 * @param {(response: ResponseType) => void} onSuccess A function to execute when the request completes successfully.
 * @param {(e: AxiosError) => void} onError A function to execute when the request throws an error.
 */
export const useApiRequestWrapper = <ResponseType>({
  requestFunction,
  requestName,
  onSuccess,
  onError,
  invoke,
}: IApiRequestWrapper<ResponseType>): IResponseWrapper<ResponseType> => {
  const [loading, setLoading] = useState<boolean | undefined>();
  const [error, setError] = useState<AxiosError<IApiError>>();
  const [response, setResponse] = useState<ResponseType>();
  const dispatch = useDispatch();
  const isMounted = useIsMounted();

  // allow direct access to the api wrapper function in case direct invocation is required by consumer.
  const wrappedApiRequest = useCallback<() => Promise<ResponseType | undefined>>(async () => {
    try {
      // reset initial state whenever a request is started.
      setLoading(true);
      setError(undefined);
      setResponse(undefined);
      const response = (await handleAxiosResponse(
        dispatch,
        requestName,
        requestFunction(),
      )) as ResponseType;
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
      setLoading(false);
    }
  }, [dispatch, isMounted, onError, onSuccess, requestFunction, requestName]);

  useEffect(() => {
    if (invoke) {
      wrappedApiRequest();
    }
  }, [wrappedApiRequest, invoke]);

  return {
    error: error,
    response: response,
    loading: loading,
    refresh: wrappedApiRequest,
  } as IResponseWrapper<ResponseType>;
};
