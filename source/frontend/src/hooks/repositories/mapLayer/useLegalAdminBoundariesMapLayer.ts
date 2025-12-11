import { FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback, useMemo } from 'react';

import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import { WHSE_AgriculturalLandReservePoly_Feature_Properties } from '@/models/layers/alcAgriculturalReserve';
import { WHSE_Municipalities_Feature_Properties } from '@/models/layers/municipalities';
import { useTenant } from '@/tenants';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints on the ALC Agricultural Land Reserve Lines layer.
 * @returns Object containing functions to make requests to the WFS layer.
 * Note: according to https://catalogue.data.gov.bc.ca/dataset/alc-agricultural-land-reserve-lines/resource/8e5099f5-58da-4cee-beab-9d09609c4596
 */
export const useLegalAdminBoundariesMapLayer = () => {
  const { alrLayerUrl, municipalLayerUrl } = useTenant();

  const {
    findMultipleWhereContainsWrapped: findOneAlrWrapped_,
    findMultipleWhereContainsBoundaryWrapped: findMultipleAlrBoundaryWrapped_,
  } = useLayerQuery(alrLayerUrl);
  const findMultipleAlrBoundaryWrappedExecute = findMultipleAlrBoundaryWrapped_.execute;
  const findMultipleAlrBoundaryWrappedLoading = findMultipleAlrBoundaryWrapped_.loading;
  const findOneAlrWrappedExecute = findOneAlrWrapped_.execute;
  const findOneAlrWrappedLoading = findOneAlrWrapped_.loading;

  const {
    findMultipleWhereContainsWrapped: findOneMunicipalWrapped_,
    findMultipleWhereContainsWrapped: findMultipleMunicipalWrapped_,
    findMultipleWhereContainsBoundaryWrapped: findMultipleMunicipalBoundaryWrapped_,
  } = useLayerQuery(municipalLayerUrl);

  const findMultipleMunicipalBoundaryWrappedExecute = findMultipleMunicipalBoundaryWrapped_.execute;
  const findMultipleMunicipalBoundaryWrappedLoading = findMultipleMunicipalBoundaryWrapped_.loading;

  const findMultipleMunicipalWrappedExecute = findMultipleMunicipalWrapped_.execute;
  const findMultipleMunicipalWrappedLoading = findMultipleMunicipalWrapped_.loading;

  const findOneMunicipalWrappedExecute = findOneMunicipalWrapped_.execute;
  const findOneMunicipalWrappedLoading = findOneMunicipalWrapped_.loading;

  const findOneAgriculturalReserve = useCallback(
    async (latlng: LatLngLiteral, geometryName?: string, spatialReferenceId?: number) => {
      const featureCollection = await findOneAlrWrappedExecute(
        latlng,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as
        | FeatureCollection<Geometry, WHSE_AgriculturalLandReservePoly_Feature_Properties>
        | undefined;
      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneAlrWrappedExecute],
  );

  const findMultipleAgriculturalReserveBoundary = useCallback(
    async (boundary: Geometry, geometryName?: string, spatialReferenceId?: number) => {
      const featureCollection = await findMultipleAlrBoundaryWrappedExecute(
        boundary,
        geometryName,
        spatialReferenceId,
      );

      return featureCollection as
        | FeatureCollection<Geometry, WHSE_AgriculturalLandReservePoly_Feature_Properties>
        | undefined;
    },
    [findMultipleAlrBoundaryWrappedExecute],
  );

  const findOneMunicipality = useCallback(
    async (latlng: LatLngLiteral, geometryName?: string, spatialReferenceId?: number) => {
      const featureCollection = await findOneMunicipalWrappedExecute(
        latlng,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as
        | FeatureCollection<Geometry, WHSE_Municipalities_Feature_Properties>
        | undefined;
      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneMunicipalWrappedExecute],
  );

  const findMultipleMunicipality = useCallback(
    async (latlng: LatLngLiteral, geometryName?: string, spatialReferenceId?: number) => {
      const featureCollection = await findMultipleMunicipalWrappedExecute(
        latlng,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as
        | FeatureCollection<Geometry, WHSE_Municipalities_Feature_Properties>
        | undefined;
      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features
        : undefined;
    },
    [findMultipleMunicipalWrappedExecute],
  );

  const findMultipleMunicipalityBoundary = useCallback(
    async (boundary: Geometry, geometryName?: string, spatialReferenceId?: number) => {
      const featureCollection = await findMultipleMunicipalBoundaryWrappedExecute(
        boundary,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as
        | FeatureCollection<Geometry, WHSE_Municipalities_Feature_Properties>
        | undefined;
      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features
        : undefined;
    },
    [findMultipleMunicipalBoundaryWrappedExecute],
  );

  return useMemo(
    () => ({
      findOneAgriculturalReserve,
      findOneAgriculturalReserveLoading: findOneAlrWrappedLoading,
      findMultipleAgriculturalReserveBoundary,
      findMultipleAgriculturalReserveLoading: findMultipleAlrBoundaryWrappedLoading,
      findMultipleMunicipality,
      findMultipleMunicipalLoading: findMultipleMunicipalWrappedLoading,
      findMultipleMunicipalityBoundary,
      findMultipleMunicipalBoundaryLoading: findMultipleMunicipalBoundaryWrappedLoading,
      findOneMunicipality,
      findOneMunicipalLoading: findOneMunicipalWrappedLoading,
    }),
    [
      findOneAgriculturalReserve,
      findOneAlrWrappedLoading,
      findMultipleAgriculturalReserveBoundary,
      findMultipleAlrBoundaryWrappedLoading,
      findMultipleMunicipality,
      findMultipleMunicipalWrappedLoading,
      findMultipleMunicipalityBoundary,
      findMultipleMunicipalBoundaryWrappedLoading,
      findOneMunicipality,
      findOneMunicipalWrappedLoading,
    ],
  );
};
