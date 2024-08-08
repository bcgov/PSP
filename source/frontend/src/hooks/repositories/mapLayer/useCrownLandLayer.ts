import { FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback } from 'react';

import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import {
  TANTALIS_CrownLandInclusions_Feature_Properties,
  TANTALIS_CrownLandInventory_Feature_Properties,
  TANTALIS_CrownLandLeases_Feature_Properties,
  TANTALIS_CrownLandLicenses_Feature_Properties,
  TANTALIS_CrownLandTenures_Feature_Properties,
} from '@/models/layers/crownLand';
import { useTenant } from '@/tenants';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints on the set of Crown Land related layers.
 * @returns Object containing functions to make requests to the WFS layer.
 */

export const useCrownLandLayer = () => {
  const {
    crownLandLeasesUrl,
    crownLandLicensesUrl,
    crownLandTenuresUrl,
    crownLandInventoryUrl,
    crownLandInclusionsUrl,
  } = useTenant();

  const {
    findOneWhereContainsWrapped: {
      execute: findOneWhereContainsCrownLandLeasesExecute,
      loading: findOneWhereContainsCrownLandLeasesLoading,
    },
  } = useLayerQuery(crownLandLeasesUrl);

  const findOneCrownLandLease = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findOneWhereContainsCrownLandLeasesExecute(
        latlng,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        TANTALIS_CrownLandLeases_Feature_Properties
      >;

      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneWhereContainsCrownLandLeasesExecute],
  );

  const {
    findOneWhereContainsWrapped: {
      execute: findOneWhereContainsCrownLandLicensesExecute,
      loading: findOneWhereContainsCrownLandLicensesLoading,
    },
  } = useLayerQuery(crownLandLicensesUrl);

  const findOneCrownLandLicense = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findOneWhereContainsCrownLandLicensesExecute(
        latlng,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        TANTALIS_CrownLandLicenses_Feature_Properties
      >;

      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneWhereContainsCrownLandLicensesExecute],
  );

  const {
    findOneWhereContainsWrapped: {
      execute: findOneWhereContainsCrownLandTenuresExecute,
      loading: findOneWhereContainsCrownLandTenuresLoading,
    },
  } = useLayerQuery(crownLandTenuresUrl);

  const findOneCrownLandTenure = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findOneWhereContainsCrownLandTenuresExecute(
        latlng,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        TANTALIS_CrownLandTenures_Feature_Properties
      >;

      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneWhereContainsCrownLandTenuresExecute],
  );

  const {
    findOneWhereContainsWrapped: {
      execute: findOneWhereContainsCrownLandInventoryExecute,
      loading: findOneWhereContainsCrownLandInventoryLoading,
    },
  } = useLayerQuery(crownLandInventoryUrl);

  const findOneCrownLandInventory = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findOneWhereContainsCrownLandInventoryExecute(
        latlng,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        TANTALIS_CrownLandInventory_Feature_Properties
      >;

      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneWhereContainsCrownLandInventoryExecute],
  );

  const {
    findOneWhereContainsWrapped: {
      execute: findOneWhereContainsCrownLandInclusionsExecute,
      loading: findOneWhereContainsCrownLandInclusionsLoading,
    },
  } = useLayerQuery(crownLandInclusionsUrl);

  const findOneCrownLandInclusion = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findOneWhereContainsCrownLandInclusionsExecute(
        latlng,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        TANTALIS_CrownLandInclusions_Feature_Properties
      >;

      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneWhereContainsCrownLandInclusionsExecute],
  );

  return {
    findOneCrownLandLease,
    findOneCrownLandLeaseLoading: findOneWhereContainsCrownLandLeasesLoading,
    findOneCrownLandLicense,
    findOneCrownLandLicenseLoading: findOneWhereContainsCrownLandLicensesLoading,
    findOneCrownLandTenure,
    findOneCrownLandTenureLoading: findOneWhereContainsCrownLandTenuresLoading,
    findOneCrownLandInventory,
    findOneCrownLandInventoryLoading: findOneWhereContainsCrownLandInventoryLoading,
    findOneCrownLandInclusion,
    findOneCrownLandInclusionsLoading: findOneWhereContainsCrownLandInclusionsLoading,
  };
};
