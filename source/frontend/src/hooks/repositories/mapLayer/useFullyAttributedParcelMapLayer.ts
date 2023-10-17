import { FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback } from 'react';

import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import { useWfsLayer } from '@/hooks/layer-api/useWfsLayer';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { useTenant } from '@/tenants';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints on the Fully Attributed ParcelMapBC layer.
 * @returns Object containing functions to make requests to the WFS layer.
 * Note: according to https://catalogue.data.gov.bc.ca/dataset/parcelmap-bc-parcel-fabric-fully-attributed/resource/59d9964f-bc93-496f-8039-b83ab8f24a41
 */

export const useFullyAttributedParcelMapLayer = () => {
  const { parcelMapFullyAttributed, parcelsLayerUrl } = useTenant();

  const getAllFeaturesWrapper = useWfsLayer(parcelMapFullyAttributed.url, {
    name: parcelMapFullyAttributed.name,
  });

  const { findOneWhereContains } = useLayerQuery(parcelsLayerUrl);

  const { execute: getAllFeatures, loading: getAllFeaturesLoading } = getAllFeaturesWrapper;

  const findByLegalDescription = useCallback(
    async (legalDesc: string) => {
      const data = await getAllFeatures({ LEGAL_DESCRIPTION: legalDesc }, { timeout: 40000 });

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      return data as
        | FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
        | undefined;
    },
    [getAllFeatures],
  );

  const findByPid = useCallback(
    async (pid: string, forceExactMatch = false) => {
      // Removes dashes to match expectations of the map layer.
      const formattedPid = pid.replace(/[-\s]/g, '');
      const data = await getAllFeatures(
        { PID: formattedPid },
        { forceExactMatch: forceExactMatch, timeout: 30000 },
      );
      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      return data as
        | FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
        | undefined;
    },
    [getAllFeatures],
  );

  const findByPin = useCallback(
    async (pin: string, forceExactMatch = false) => {
      const data = await getAllFeatures(
        { PIN: pin },
        { forceExactMatch: forceExactMatch, timeout: 30000 },
      );
      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      return data as
        | FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
        | undefined;
    },
    [getAllFeatures],
  );

  const findByPlanNumber = useCallback(
    async (planNumber: string, forceExactMatch = false) => {
      const data = await getAllFeatures(
        { PLAN_NUMBER: planNumber },
        { forceExactMatch: forceExactMatch, timeout: 30000 },
      );
      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      return data as
        | FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
        | undefined;
    },
    [getAllFeatures],
  );

  const findOne = useCallback(
    async (latlng: LatLngLiteral, geometryName?: string, spatialReferenceId?: number) => {
      const featureCollection = await findOneWhereContains(
        latlng,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as
        | FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
        | undefined;
      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneWhereContains],
  );

  return {
    findByLegalDescription,
    findByPid,
    findByPin,
    findByPlanNumber,
    findByLoading: getAllFeaturesLoading,
    findByWrapper: getAllFeaturesWrapper,
    findOne,
  };
};
