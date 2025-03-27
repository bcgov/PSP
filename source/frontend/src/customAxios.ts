import { Attributes, Span, SpanStatusCode } from '@opentelemetry/api';
import { ATTR_HTTP_REQUEST_METHOD, ATTR_URL_FULL } from '@opentelemetry/semantic-conventions';
import { Dispatch } from '@reduxjs/toolkit';
import axios, { AxiosError, AxiosRequestHeaders } from 'axios';
import isEmpty from 'lodash/isEmpty';
import { hideLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';

import { IGenericNetworkAction } from '@/store/slices/network/interfaces';
import { logError } from '@/store/slices/network/networkSlice';
import { RootState, store } from '@/store/store';

import { SpanEnrichment } from './telemetry/SpanEnrichment';
import { startTrace } from './telemetry/traces';
import { buildUrl } from './telemetry/utils';
import { exists } from './utils';

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
  let span: Span;
  const instance = axios.create({
    baseURL,
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Cache-Control': import.meta.env.DEV ? 'no-cache' : '',
    },
  });
  instance.interceptors.request.use(config => {
    // start a trace to measure round-trip time (RTT) of network requests
    const method = (config.method || 'GET').toUpperCase();
    const urlString = [config.baseURL, config.url].filter(exists).join('');
    const url = buildUrl(urlString);
    const spanAttributes: Attributes = {
      component: 'axios',
      [ATTR_HTTP_REQUEST_METHOD]: method,
      [ATTR_URL_FULL]: url.href || '', // store full url (including query params in the span metadata)
    };
    // clear query parameters - we don't want to include them in the span name
    url.search = '';
    const spanName = `HTTP ${method} ${url.href}`;
    span = startTrace(spanName, spanAttributes);

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
      SpanEnrichment.enrichWithXhrResponse(span, response);
      span.setStatus({ code: SpanStatusCode.OK });
      span.end();

      if (lifecycleToasts?.successToast && response.status < 300) {
        loadingToastId && toast.dismiss(loadingToastId);
        lifecycleToasts.successToast();
      } else if (lifecycleToasts?.errorToast && response.status >= 300) {
        lifecycleToasts.errorToast();
      }
      return response;
    },
    error => {
      span.recordException(error);
      span.setStatus({ code: SpanStatusCode.ERROR });

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
