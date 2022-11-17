import { AxiosError, AxiosResponse } from 'axios';
import { toCqlFilterValue } from 'components/maps/leaflet/mapUtils';
import { layerData } from 'constants/toasts';
import { IApiRequestWrapper, useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import useDeepCompareCallback from 'hooks/useDeepCompareCallback';
import isAbsoluteUrl from 'is-absolute-url';

import { fetchWithRetryTimeout } from './../../utils/fetchTimeout';

export interface IUseWfsLayerOptions {
  name?: string;
  service?: string;
  version?: string;
  outputFormat?: string;
  outputSrsName?: string;
  withCredentials?: boolean;
}

export interface IWfsGetAllFeaturesOptions {
  maxCount?: number;
  timeout?: number;
  forceSimplePid?: boolean;
  forceExactMatch?: boolean;
  onLayerError?: () => void;
}

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints.
 * @returns Object containing functions to make requests to the WFS layer.
 */
export const useWfsLayer = (
  url: string,
  layerOptions: IUseWfsLayerOptions,
  requestWrapperOptions?: Partial<IApiRequestWrapper<any>>,
) => {
  const getAllFeaturesWrapper = useApiRequestWrapper({
    ...(requestWrapperOptions ?? {}),
    requestFunction: useDeepCompareCallback(
      async (filter: Record<string, string> = {}, options?: IWfsGetAllFeaturesOptions) => {
        const urlObj = buildUrl(url, getUrlParams(layerOptions));

        // add extra WFS params
        urlObj.searchParams.set('request', 'GetFeature');
        if (options?.maxCount !== undefined) {
          urlObj.searchParams.set('count', options.maxCount.toString());
        }
        const cqlFilter = toCqlFilterValue(
          filter,
          options?.forceSimplePid,
          options?.forceExactMatch,
        );
        if (cqlFilter) {
          urlObj.searchParams.set('cql_filter', cqlFilter);
        }
        const response = await fetchWithRetryTimeout({
          credentials: layerOptions.withCredentials ? 'include' : 'same-origin',
          url: urlObj.href,
          timeout: options?.timeout ?? 5000,
          redirect: 'error',
          onError: options?.onLayerError ?? (() => layerData.LAYER_DATA_ERROR()),
        });
        if (!response.ok) {
          const error: Partial<AxiosError> = {
            isAxiosError: true,
            response: {
              data: {},
              status: response.status,
              statusText: response.statusText,
              headers: {},
              config: {},
            },
          };
          throw error;
        }
        const data: AxiosResponse = {
          data: await response.json(),
          status: response.status,
          statusText: response.statusText,
          headers: {},
          config: {},
        };
        return data;
      },
      [layerOptions, url],
    ),
    requestName: requestWrapperOptions?.requestName ?? 'getAllFeatures',
  });

  return getAllFeaturesWrapper;
};

function getUrlParams(options: IUseWfsLayerOptions): Record<string, any> {
  const mergedParams = Object.assign<Partial<IUseWfsLayerOptions>, IUseWfsLayerOptions>(
    {
      service: 'WFS',
      version: '2.0.0',
      outputFormat: 'json',
      outputSrsName: 'EPSG:4326',
    },
    options,
  );

  return {
    service: mergedParams.service,
    version: mergedParams.version,
    outputFormat: mergedParams.outputFormat,
    typeNames: mergedParams.name,
    srsName: mergedParams.outputSrsName,
  };
}

// creates URL and appends query parameters
function buildUrl(inputUrl: string, queryParams: Record<string, any> = {}): URL {
  const baseUrl = window.location.origin;
  const urlObj = isAbsoluteUrl(inputUrl) ? new URL(inputUrl) : new URL(inputUrl, baseUrl);
  Object.keys(queryParams).forEach(k => {
    if (queryParams[k] !== undefined) {
      urlObj.searchParams.set(k, queryParams[k]);
    }
  });
  return urlObj;
}
