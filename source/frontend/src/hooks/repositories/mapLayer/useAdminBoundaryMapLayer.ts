import { FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback, useMemo } from 'react';

import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import { EBC_ELECTORAL_DISTS_BS10_SVW_Feature_Properties } from '@/models/layers/electoralBoundaries';
import { MOT_DistrictBoundary_Feature_Properties } from '@/models/layers/motDistrictBoundary';
import { MOT_RegionalBoundary_Feature_Properties } from '@/models/layers/motRegionalBoundary';
import { useTenant } from '@/tenants';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints on WHSE Admin boundaries layer.
 * @returns Object containing functions to make requests to the WFS layer.
 * Note: according to https://catalogue.data.gov.bc.ca/dataset/ministry-of-transportation-mot-region-boundary
 *                  & https://catalogue.data.gov.bc.ca/dataset/ministry-of-transportation-mot-district-boundary
 *                  & https://catalogue.data.gov.bc.ca/dataset/provincial-electoral-districts-electoral-boundaries-redistribution-2015/
 */
export const useAdminBoundaryMapLayer = () => {
  const tenant = useTenant();
  const {
    findOneWhereContainsWrapped: findOneWhereContainsRegion,
    findOneWhereContainsLoading: regionSearchLoading,
  } = useLayerQuery(tenant.motiRegionLayerUrl);
  const {
    findOneWhereContainsWrapped: findOneWhereContainsDistrict,
    findOneWhereContainsLoading: districtSearchLoading,
  } = useLayerQuery(tenant.hwyDistrictLayerUrl);

  const findRegion = useCallback(
    async (latlng: LatLngLiteral, geometryName: string = 'SHAPE', spatialReferenceId = 4326) => {
      const featureCollection = await findOneWhereContainsRegion(
        latlng,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        MOT_RegionalBoundary_Feature_Properties
      >;
      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneWhereContainsRegion],
  );

  const findDistrict = useCallback(
    async (latlng: LatLngLiteral, geometryName: string = 'SHAPE', spatialReferenceId = 4326) => {
      const featureCollection = await findOneWhereContainsDistrict(
        latlng,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        MOT_DistrictBoundary_Feature_Properties
      >;
      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneWhereContainsDistrict],
  );

  const { findOneWhereContains: electoralApi, findOneWhereContainsLoading: electoralLoading } =
    useLayerQuery(tenant.electoralLayerUrl);

  const findElectoralDistrict = useCallback(
    async (latlng: LatLngLiteral, geometryName?: string, spatialReferenceId?: number) => {
      const featureCollection = await electoralApi(latlng, geometryName, spatialReferenceId);

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as
        | FeatureCollection<Geometry, EBC_ELECTORAL_DISTS_BS10_SVW_Feature_Properties>
        | undefined;
      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [electoralApi],
  );

  return useMemo(
    () => ({
      findRegion,
      findDistrict,
      findElectoralDistrict,
      findRegionLoading: regionSearchLoading,
      findDistrictLoading: districtSearchLoading,
      findElectoralDistrictLoading: electoralLoading,
    }),
    [
      findRegion,
      findDistrict,
      findElectoralDistrict,
      regionSearchLoading,
      districtSearchLoading,
      electoralLoading,
    ],
  );
};
