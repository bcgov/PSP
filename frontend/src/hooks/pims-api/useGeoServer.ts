import { toCqlFilter } from 'components/maps/leaflet/mapUtils';
import CustomAxios from 'customAxios';
import { Feature, FeatureCollection, Point } from 'geojson';
import { useCallback, useContext } from 'react';
import { TenantContext } from 'tenants';

function buildUrl(inputUrl: string, id: number) {
  return `${inputUrl}${id ? toCqlFilter({ PROPERTY_ID: id }) : ''}`;
}

/**
 * API wrapper to centralize all AJAX requests to GeoServer endpoints.
 * @returns Object containing functions to make requests to the GeoServer API.
 */
export const useGeoServer = () => {
  const { tenant } = useContext(TenantContext);
  const url = tenant.propertiesUrl || '';

  const getPropertyWfs = useCallback(
    async function(id: number): Promise<Feature<Point> | null> {
      const { data } = await CustomAxios().get<FeatureCollection>(buildUrl(url, id));
      return data.features?.length ? (data.features[0] as Feature<Point>) : null;
    },
    [url],
  );

  return { getPropertyWfs };
};
