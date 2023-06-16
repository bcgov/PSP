import { FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback, useMemo } from 'react';

import { useWfsLayer } from '@/hooks/layer-api/useWfsLayer';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { useTenant } from '@/tenants';

import { useLayerQuery } from '../useLayerQuery';

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

  const { findOneWhereContainsWrapped, findOneWhereContainsLoading } =
    useLayerQuery(parcelsLayerUrl);

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
      const formattedPid = pid.replace(/-/g, '');
      const data = await getAllFeatures(
        { PID: formattedPid },
        { forceSimplePid: true, forceExactMatch: forceExactMatch, timeout: 30000 },
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
    async (planNumber: string) => {
      const data = await getAllFeatures({ PLAN_NUMBER: planNumber }, { timeout: 30000 });
      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      return data as
        | FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
        | undefined;
    },
    [getAllFeatures],
  );

  const findOne = useCallback(
    async (latlng: LatLngLiteral, geometryName?: string, spatialReferenceId?: number) => {
      const featureCollection = await findOneWhereContainsWrapped(
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
    [findOneWhereContainsWrapped],
  );

  return useMemo(
    () => ({
      findByLegalDescription,
      findByPid,
      findByPin,
      findByPlanNumber,
      findByLoading: getAllFeaturesLoading,
      findByWrapper: getAllFeaturesWrapper,
      findOne,
      findOneLoading: findOneWhereContainsLoading,
    }),
    [
      findByLegalDescription,
      findByPid,
      findByPin,
      findByPlanNumber,
      getAllFeaturesLoading,
      getAllFeaturesWrapper,
      findOne,
      findOneWhereContainsLoading,
    ],
  );
};
