import { toCqlFilter } from 'components/maps/leaflet/mapUtils';
import CustomAxios from 'customAxios';
import { Feature, FeatureCollection, Point } from 'geojson';
import { useCallback, useContext } from 'react';
import { TenantContext } from 'tenants';

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
    async function(id: number): Promise<Feature<Point> | null> {
      if (isNaN(Number(id))) {
        return null;
      }
      const wfsUrl = buildUrl(baseUrl, { PROPERTY_ID: id });
      const { data } = await CustomAxios().get<FeatureCollection>(wfsUrl);
      return data.features?.length ? (data.features[0] as Feature<Point>) : null;
    },
    [baseUrl],
  );

  const getPropertyWithPidWfs = useCallback(
    async function(pid: string): Promise<Feature<Point> | null> {
      if (!pid) {
        return null;
      }
      const wfsUrl = buildUrl(baseUrl, { PID_PADDED: pid });
      const { data } = await CustomAxios().get<FeatureCollection>(wfsUrl);
      return data.features?.length ? (data.features[0] as Feature<Point>) : null;
    },
    [baseUrl],
  );

  return { getPropertyWfs, getPropertyWithPidWfs };
};
