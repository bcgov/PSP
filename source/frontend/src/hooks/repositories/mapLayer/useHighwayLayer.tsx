import { FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback, useContext, useMemo } from 'react';

import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import { ISS_ProvincialPublicHighway } from '@/models/layers/pimsHighwayLayer';
import { TenantContext } from '@/tenants';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints for the highway layer location.
 * @returns Object containing functions to make requests to the WFS layer.
 * Note: according to the view ISS_ProvincialPublicHighway
 */
export const usePimsHighwayLayer = () => {
  const {
    tenant: { highwayLayerUrl },
  } = useContext(TenantContext);

  const {
    findMultipleWhereContainsWrapped: {
      execute: findMultipleWhereContainsWrappedExecute,
      loading: findMultipleWhereContainsWrappedLoading,
    },
  } = useLayerQuery(highwayLayerUrl, true);

  const findMultiple = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findMultipleWhereContainsWrappedExecute(
        latlng,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        ISS_ProvincialPublicHighway
      >;

      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features
        : undefined;
    },
    [findMultipleWhereContainsWrappedExecute],
  );

  return useMemo(
    () => ({
      findMultiple,
      findMultipleLoading: findMultipleWhereContainsWrappedLoading,
    }),
    [findMultiple, findMultipleWhereContainsWrappedLoading],
  );
};
