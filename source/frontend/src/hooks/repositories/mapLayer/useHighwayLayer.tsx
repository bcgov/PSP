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
      loading: findMultipleHighwayWhereContainsWrappedLoading,
    },
    findMultipleWhereContainsBoundaryWrapped: {
      execute: findMultipleWhereContainsBoundaryWrappedExecute,
      loading: findMultipleHighwayWhereContainsBoundaryWrappedLoading,
    },
    findBySurveyPlanNumber: findBySurveyPlanNumberApi,
    findBySurveyPlanNumberLoading: findBySurveyPlanNumberApiLoadingApi,
  } = useLayerQuery(highwayLayerUrl, true);

  const findMultipleHighway = useCallback(
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

  const findMultipleHighwayBoundary = useCallback(
    async (
      boundary: Geometry,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findMultipleWhereContainsBoundaryWrappedExecute(
        boundary,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        ISS_ProvincialPublicHighway
      >;

      return forceCasted !== undefined && forceCasted?.features?.length > 0
        ? forceCasted.features
        : undefined;
    },
    [findMultipleWhereContainsBoundaryWrappedExecute],
  );

  const findBySurveyPlanNumber = useCallback(
    async (planNumber: string, allBy?: boolean) => {
      const featureCollection = await findBySurveyPlanNumberApi(planNumber, allBy);

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        ISS_ProvincialPublicHighway
      >;

      return forceCasted !== undefined && forceCasted?.features?.length > 0
        ? forceCasted
        : undefined;
    },
    [findBySurveyPlanNumberApi],
  );

  return useMemo(
    () => ({
      findMultipleHighway: findMultipleHighway,
      findMultipleHighwayWhereContainsWrappedLoading:
        findMultipleHighwayWhereContainsWrappedLoading,
      findMultipleHighwayBoundary: findMultipleHighwayBoundary,
      findMultipleHighwayWhereContainsBoundaryWrappedLoading:
        findMultipleHighwayWhereContainsBoundaryWrappedLoading,
      findBySurveyPlanNumber,
      findBySurveyPlanNumberLoading: findBySurveyPlanNumberApiLoadingApi,
    }),
    [
      findBySurveyPlanNumber,
      findBySurveyPlanNumberApiLoadingApi,
      findMultipleHighway,
      findMultipleHighwayBoundary,
      findMultipleHighwayWhereContainsBoundaryWrappedLoading,
      findMultipleHighwayWhereContainsWrappedLoading,
    ],
  );
};
