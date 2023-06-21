import { FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback, useMemo } from 'react';

import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import { ADM_IndianReserveBands_Feature_Properties } from '@/models/layers/admIndianReserveBands';
import { useTenant } from '@/tenants';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints on the Indian Reserves and Band Names - Administrative Boundaries layer.
 * @returns Object containing functions to make requests to the WFS layer.
 * Note: according to https://catalogue.data.gov.bc.ca/dataset/indian-reserves-and-band-names-administrative-boundaries/resource/1e25495b-22b2-49e4-b81d-a97e0510255b
 */
export const useIndianReserveBandMapLayer = () => {
  const { reservesLayerUrl } = useTenant();

  const { findOneWhereContainsWrapped } = useLayerQuery(reservesLayerUrl);
  const findOneWhereContainsWrappedExecute = findOneWhereContainsWrapped.execute;
  const findOneWhereContainsWrappedLoading = findOneWhereContainsWrapped.loading;

  const findOne = useCallback(
    async (latlng: LatLngLiteral, geometryName?: string, spatialReferenceId?: number) => {
      const featureCollection = await findOneWhereContainsWrappedExecute(
        latlng,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as
        | FeatureCollection<Geometry, ADM_IndianReserveBands_Feature_Properties>
        | undefined;
      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneWhereContainsWrappedExecute],
  );

  return useMemo(
    () => ({
      findOne,
      findOneLoading: findOneWhereContainsWrappedLoading,
    }),
    [findOne, findOneWhereContainsWrappedLoading],
  );
};
