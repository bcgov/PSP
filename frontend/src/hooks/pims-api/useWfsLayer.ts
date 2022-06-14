import { toCqlFilterValue } from 'components/maps/leaflet/mapUtils';
import { FeatureCollection } from 'geojson';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { useCallback, useMemo } from 'react';

import { wfsAxios } from './wfsAxios';

export interface IUseWfsLayerOptions {
  name: string;
  service?: string;
  version?: string;
  outputFormat?: string;
  outputSrsName?: string;
}

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints.
 * @returns Object containing functions to make requests to the WFS layer.
 */
export const useWfsLayer = (url: string, layerOptions: IUseWfsLayerOptions) => {
  const { execute: getAllFeatures, loading: getAllFeaturesLoading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (
        filter: Record<string, any>,
        options: { maxCount?: number; timeout?: number; pidOverride?: boolean },
      ) => {
        const urlObj = buildUrl(url, getUrlParams(layerOptions));
        // add extra WFS params
        urlObj.searchParams.set('request', 'GetFeature');
        const cqlFilter = toCqlFilterValue(filter, options?.pidOverride);
        if (cqlFilter) {
          urlObj.searchParams.set('cql_filter', cqlFilter);
        }
        // call WFS service
        const data = await wfsAxios(options?.timeout).get<FeatureCollection>(urlObj.href);
        return data;
      },
      [layerOptions, url],
    ),
    requestName: 'getAllFeatures',
  });

  return { getAllFeatures, getAllFeaturesLoading };
};

function getUrlParams(options: IUseWfsLayerOptions): Record<string, any> {
  const mergedParams = Object.assign<Partial<IUseWfsLayerOptions>, IUseWfsLayerOptions>(
    {
      service: 'WFS',
      version: '1.3.0',
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
  var urlObj = new URL(inputUrl);
  Object.keys(queryParams).forEach(k => urlObj.searchParams.set(k, queryParams[k]));
  return urlObj;
}
