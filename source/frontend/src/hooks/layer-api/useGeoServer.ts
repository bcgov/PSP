import { Feature, FeatureCollection, Point } from 'geojson';
import { useCallback, useContext } from 'react';

import CustomAxios from '@/customAxios';
import { TenantContext } from '@/tenants';
import { useAxiosErrorHandler } from '@/utils';

import { useApiRequestWrapper } from '../util/useApiRequestWrapper';
import { toCqlFilter } from './layerUtils';

function buildUrl(inputUrl: string, cqlFilter: Record<string, any>) {
  return `${inputUrl}${toCqlFilter(cqlFilter)}`;
}

/**
 * API wrapper to centralize all AJAX requests to GeoServer endpoints.
 * @returns Object containing functions to make requests to the GeoServer API.
 */
export const useGeoServer = () => {
  const { tenant } = useContext(TenantContext);
  const baseUrl = tenant.propertiesUrl || '';

  const getPropertyWfs = useCallback(
    async function (id: number): Promise<Feature<Point> | null> {
      if (isNaN(Number(id))) {
        return null;
      }
      const wfsUrl = buildUrl(baseUrl, { PROPERTY_ID: id });
      const { data } = await CustomAxios().get<FeatureCollection>(wfsUrl);
      return data.features?.length ? (data.features[0] as Feature<Point>) : null;
    },
    [baseUrl],
  );
  const getPropertyWfsWrapper = useApiRequestWrapper({
    requestFunction: useCallback(
      async (id: number) => {
        const wfsUrl = buildUrl(baseUrl, { PROPERTY_ID: id });
        return await CustomAxios().get<FeatureCollection>(wfsUrl);
      },
      [baseUrl],
    ),
    requestName: 'getPropertyWfs',
    onError: useAxiosErrorHandler('Failed to retrieve property information from BC Data Warehouse'),
  });

  return { getPropertyWfs, getPropertyWfsWrapper };
};
