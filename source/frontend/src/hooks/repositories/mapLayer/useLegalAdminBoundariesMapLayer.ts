import { FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback, useMemo } from 'react';

import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import { WHSE_AgriculturalLandReserve_Feature_Properties } from '@/models/layers/alcAgriculturalReserveLines';
import { WHSE_Municipalities_Feature_Properties } from '@/models/layers/municipalities';
import { useTenant } from '@/tenants';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints on the ALC Agricultural Land Reserve Lines layer.
 * @returns Object containing functions to make requests to the WFS layer.
 * Note: according to https://catalogue.data.gov.bc.ca/dataset/alc-agricultural-land-reserve-lines/resource/8e5099f5-58da-4cee-beab-9d09609c4596
 */
export const useLegalAdminBoundariesMapLayer = () => {
  const { alrLayerUrl, municipalLayerUrl } = useTenant();

  const { findOneWhereContains: findOneAlR, findOneWhereContainsLoading: findOneAlRLoading } =
    useLayerQuery(alrLayerUrl);

  const {
    findOneWhereContains: findOneMunicipal,
    findOneWhereContainsLoading: findOneMunicipalLoading,
  } = useLayerQuery(municipalLayerUrl);

  const findOneAgriculturalReserve = useCallback(
    async (latlng: LatLngLiteral, geometryName?: string, spatialReferenceId?: number) => {
      const featureCollection = await findOneAlR(latlng, geometryName, spatialReferenceId);

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as
        | FeatureCollection<Geometry, WHSE_AgriculturalLandReserve_Feature_Properties>
        | undefined;
      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneAlR],
  );

  const findOneMunicipality = useCallback(
    async (latlng: LatLngLiteral, geometryName?: string, spatialReferenceId?: number) => {
      const featureCollection = await findOneMunicipal(latlng, geometryName, spatialReferenceId);

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as
        | FeatureCollection<Geometry, WHSE_Municipalities_Feature_Properties>
        | undefined;
      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneMunicipal],
  );

  return useMemo(
    () => ({
      findOneAgriculturalReserve,
      findOneAgriculturalReserveLoading: findOneAlRLoading,
      findOneMunicipality,
      findOneMunicipalLoading,
    }),
    [findOneAgriculturalReserve, findOneAlRLoading, findOneMunicipality, findOneMunicipalLoading],
  );
};
