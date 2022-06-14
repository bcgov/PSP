import { toCqlFilterValue } from 'components/maps/leaflet/mapUtils';
import { FeatureCollection } from 'geojson';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { useCallback, useMemo } from 'react';

import { wfsAxios } from './wfsAxios';

export interface IUseWfsLayerOptions {
  name: string;
  version?: string;
  outputFormat?: string;
  outputSrsName?: string;
  geometryFieldName?: string;
}

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints.
 * @returns Object containing functions to make requests to the WFS layer.
 */
export const useWfsLayer = (url: string, options: IUseWfsLayerOptions) => {
  const _layerOptions = useMemo(
    () =>
      Object.assign<Partial<IUseWfsLayerOptions>, IUseWfsLayerOptions>(
        {
          version: '1.3.0',
          outputFormat: 'json',
          outputSrsName: 'EPSG:4326',
          geometryFieldName: 'SHAPE',
        },
        options,
      ),
    [options],
  );

  const { execute: getAllFeatures, loading: getAllFeaturesLoading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (
        filter: Record<string, any>,
        options: { maxCount?: number; timeout?: number; pidOverride?: boolean },
      ) => {
        const urlObj = buildUrl(url, _layerOptions);
        const cqlFilter = toCqlFilterValue(filter, options?.pidOverride);
        if (cqlFilter) {
          urlObj.searchParams.set('cql_filter', cqlFilter);
        }
        const data = await wfsAxios(options?.timeout).get<FeatureCollection>(urlObj.href);
        return data;
      },
      [_layerOptions, url],
    ),
    requestName: 'getAllFeatures',
  });

  return { getAllFeatures, getAllFeaturesLoading };
};

function buildUrl(inputUrl: string, options: IUseWfsLayerOptions): URL {
  var urlObj = new URL(inputUrl);
  urlObj.searchParams.set('typeName', options.name);
  if (options.outputFormat) {
    urlObj.searchParams.set('outputformat', options.outputFormat);
  }
  if (options.outputSrsName) {
    urlObj.searchParams.set('srsName', options.outputSrsName);
  }
  if (options.version) {
    urlObj.searchParams.set('version', options.version);
  }
  return urlObj;
}
