import { AxiosError, AxiosPromise } from 'axios';
import { useCallback, useState } from 'react';
import { useDispatch } from 'react-redux';
import { handleAxiosResponse } from 'utils';

export interface IResponseWrapper<ResponseType> {
  error: AxiosError;
  response: ResponseType;
  wrappedRequestFunction: () => Promise<ResponseType | undefined>;
  loading?: boolean;
}

/**
 * Wrapper for api requests that tracks the state of the response. Also returns the wrapped api request directly.
 * @param requestFunction Any promise that returns an axios response an success and an axios error on failure.
 * @param requestName A unique name used to identify this wrapped request.
 * @param onSuccess A function to execute when the request completes successfully.
 * @param onError A function to execute when the request throws an error.
 */
export const useApiRequestWrapper = <ResponseType>(
  requestFunction: () => AxiosPromise<ResponseType>,
  requestName: string,
  onSuccess?: (response: ResponseType) => void,
  onError?: (e: AxiosError) => void,
): IResponseWrapper<ResponseType> => {
  const [loading, setLoading] = useState<boolean | undefined>();
  const [error, setError] = useState<AxiosError>();
  const [response, setResponse] = useState<ResponseType>();
  const dispatch = useDispatch();

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
      setResponse(response);
      onSuccess && onSuccess(response);
      return response;
    } catch (e) {
      onError && onError(e);
      setError(e);
    } finally {
      setLoading(false);
    }
  }, [dispatch, onError, onSuccess, requestFunction, requestName]);

  return {
    error: error,
    response: response,
    loading: loading,
    wrappedRequestFunction: wrappedApiRequest,
  } as IResponseWrapper<ResponseType>;
};
