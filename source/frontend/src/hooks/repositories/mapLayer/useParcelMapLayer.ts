import { FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import { useWfsLayer } from '@/hooks/layer-api/useWfsLayer';
import { PMBC_Feature_Properties } from '@/models/layers/parcelMapBC';
import { useTenant } from '@/tenants';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints on the Fully Attributed ParcelMapBC layer.
 * @returns Object containing functions to make requests to the WFS layer.
 * Note: according to https://catalogue.data.gov.bc.ca/dataset/parcelmap-bc-parcel-fabric-fully-attributed/resource/59d9964f-bc93-496f-8039-b83ab8f24a41
 */

export const useParcelMapLayer = () => {
  const { parcelMap, parcelsLayerUrl } = useTenant();

  const getAllFeaturesWrapper = useWfsLayer(parcelMap.url, {
    name: parcelMap.name,
  });

  const { findOneWhereContains } = useLayerQuery(parcelsLayerUrl);

  const { execute: getAllFeatures, loading: getAllFeaturesLoading } = getAllFeaturesWrapper;

  const handleError = useCallback(() => {
    toast.error('Unable to contact Parcel Map');
  }, []);

  const findByPid = useCallback(
    async (pid: string, forceExactMatch = false) => {
      // Removes dashes to match expectations of the map layer.
      const formattedPid = pid.replace(/[-\s]/g, '');
      try {
        const data = await getAllFeatures(
          { PID: formattedPid },
          { forceExactMatch: forceExactMatch, timeout: 30000 },
        );
        // TODO: Enhance useLayerQuery to allow generics to match the Property types
        return data as FeatureCollection<Geometry, PMBC_Feature_Properties> | undefined;
      } catch (e: unknown) {
        handleError();
        return undefined;
      }
    },
    [getAllFeatures, handleError],
  );

  const findByPin = useCallback(
    async (pin: string, forceExactMatch = false) => {
      try {
        const data = await getAllFeatures(
          { PIN: pin },
          { forceExactMatch: forceExactMatch, timeout: 30000 },
        );
        // TODO: Enhance useLayerQuery to allow generics to match the Property types
        return data as FeatureCollection<Geometry, PMBC_Feature_Properties> | undefined;
      } catch (e: unknown) {
        handleError();
        return undefined;
      }
    },
    [getAllFeatures, handleError],
  );

  const findByPlanNumber = useCallback(
    async (planNumber: string, forceExactMatch = false) => {
      try {
        const data = await getAllFeatures(
          { PLAN_NUMBER: planNumber },
          { forceExactMatch: forceExactMatch, timeout: 30000 },
        );
        // TODO: Enhance useLayerQuery to allow generics to match the Property types
        return data as FeatureCollection<Geometry, PMBC_Feature_Properties> | undefined;
      } catch (e: unknown) {
        handleError();
        return undefined;
      }
    },
    [getAllFeatures, handleError],
  );

  const findOne = useCallback(
    async (latlng: LatLngLiteral, geometryName?: string, spatialReferenceId?: number) => {
      try {
        const featureCollection = await findOneWhereContains(
          latlng,
          geometryName,
          spatialReferenceId,
        );

        // TODO: Enhance useLayerQuery to allow generics to match the Property types
        const forceCasted = featureCollection as
          | FeatureCollection<Geometry, PMBC_Feature_Properties>
          | undefined;
        return forceCasted !== undefined && forceCasted.features.length > 0
          ? forceCasted.features[0]
          : undefined;
      } catch (e: unknown) {
        handleError();
        return undefined;
      }
    },
    [findOneWhereContains, handleError],
  );

  return {
    findByPid,
    findByPin,
    findByPlanNumber,
    findByLoading: getAllFeaturesLoading,
    findByWrapper: getAllFeaturesWrapper,
    findOne,
  };
};
